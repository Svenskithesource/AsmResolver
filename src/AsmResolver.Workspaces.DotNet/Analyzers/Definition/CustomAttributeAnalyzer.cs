using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;

namespace AsmResolver.Workspaces.DotNet.Analyzers.Definition
{
    /// <summary>
    /// Analyzes a <see cref="CustomAttribute"/> for its definitions
    /// </summary>
    public class CustomAttributeAnalyzer : ObjectAnalyzer<CustomAttribute>
    {
        private static readonly SignatureComparer _comparer = new();

        /// <inheritdoc />
        public override void Analyze(AnalysisContext context, CustomAttribute subject)
        {
            ScheduleMembersForAnalysis(context, subject);
            ConnectAllNamedReferences(context, subject);
        }

        private void ScheduleMembersForAnalysis(AnalysisContext context, CustomAttribute subject)
        {
            if (context.HasAnalyzers(typeof(CustomAttribute)))
            {
                context.SchedulaForAnalysis(subject);
            }

            if (context.HasAnalyzers(subject.Constructor.GetType()))
            {
                context.SchedulaForAnalysis(subject.Constructor);
            }

            for (int i = 0; i < subject.Signature.NamedArguments.Count; i++)
            {
                var namedArgument = subject.Signature.NamedArguments[i];
                if (context.HasAnalyzers(typeof(CustomAttributeNamedArgument)))
                {
                    context.SchedulaForAnalysis(namedArgument);
                }
            }

            for (int i = 0; i < subject.Signature.FixedArguments.Count; i++)
            {
                var fixedArgument = subject.Signature.FixedArguments[i];
                if (context.HasAnalyzers(typeof(CustomAttributeArgument)))
                {
                    context.SchedulaForAnalysis(fixedArgument);
                }
            }
        }

        private void ConnectAllNamedReferences(AnalysisContext context, CustomAttribute subject)
        {
            if (context.Workspace is not DotNetWorkspace workspace)
                return;

            var index = context.Workspace.Index;

            var type = subject.Constructor.DeclaringType?.Resolve();
            if (type is null || !workspace.Assemblies.Contains(type.Module.Assembly))
                return;

            for (int i = 0; i < subject.Signature.NamedArguments.Count; i++)
            {
                var namedArgument = subject.Signature.NamedArguments[i];
                var member = FindMember(type, subject, namedArgument);
                if (member is null)
                    continue; //TODO: Log error?
                var node = index.GetOrCreateNode(member);
                var candidateNode = index.GetOrCreateNode(namedArgument);
                node.AddRelation(DotNetRelations.ReferenceArgument, candidateNode);
            }
        }

        private IMetadataMember? FindMember(TypeDefinition type, CustomAttribute customAttribute,
            CustomAttributeNamedArgument argument)
            => argument.MemberType switch
            {
                CustomAttributeArgumentMemberType.Property => FindNameReferenceProperty(type, argument),
                CustomAttributeArgumentMemberType.Field => FindNameReferenceField(type, argument),
                _ => null
            };

        private static IMetadataMember? FindNameReferenceProperty(TypeDefinition type,
            CustomAttributeNamedArgument argument)
        {
            for (int i = 0; i < type.Properties.Count; i++)
            {
                var field = type.Properties[i];
                if (field.Name != argument.MemberName)
                    continue;
                if (!_comparer.Equals(field.Signature.ReturnType, argument.ArgumentType))
                    continue;
                return field;
            }

            return null;
        }

        private static IMetadataMember? FindNameReferenceField(TypeDefinition type,
            CustomAttributeNamedArgument argument)
        {
            for (int i = 0; i < type.Fields.Count; i++)
            {
                var field = type.Fields[i];
                if (field.Name != argument.MemberName)
                    continue;
                if (!_comparer.Equals(field.Signature.FieldType, argument.ArgumentType))
                    continue;
                return field;
            }

            return null;
        }
    }
}
