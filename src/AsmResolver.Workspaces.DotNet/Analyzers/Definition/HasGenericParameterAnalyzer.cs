using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;

namespace AsmResolver.Workspaces.DotNet.Analyzers.Definition
{
    /// <summary>
    /// Provides a default implementation for an <see cref="IHasGenericParameters"/> analyzer.
    /// </summary>
    public class HasGenericParameterAnalyzer : ObjectAnalyzer<IHasGenericParameters>
    {
        /// <inheritdoc />
        public override void Analyze(AnalysisContext context, IHasGenericParameters subject)
        {
            bool hasGenericParameterAnalyzer = context.HasAnalyzers(typeof(GenericParameter));
            bool hasGenericParameterConstraintAnalyzer = context.HasAnalyzers(typeof(GenericParameter));
            
            for (int i = 0; i < subject.GenericParameters.Count; i++)
            {
                var genericParameter = subject.GenericParameters[i];
                if (hasGenericParameterAnalyzer)
                {
                    context.SchedulaForAnalysis(genericParameter);
                }

                for (int j = 0; j < genericParameter.Constraints.Count; j++)
                {
                    var parameterConstraint = genericParameter.Constraints[j];
                    if (hasGenericParameterConstraintAnalyzer)
                    {
                        context.SchedulaForAnalysis(parameterConstraint);
                    }

                    if (parameterConstraint.Constraint is not null
                        && context.HasAnalyzers(parameterConstraint.Constraint.GetType()))
                    {
                        context.SchedulaForAnalysis(parameterConstraint.Constraint);
                    }
                }
            }
        }
    }
}