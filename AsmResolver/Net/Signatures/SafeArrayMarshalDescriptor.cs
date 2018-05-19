﻿using AsmResolver.Net.Emit;

namespace AsmResolver.Net.Signatures
{
    public class SafeArrayMarshalDescriptor : MarshalDescriptor
    {
        public new static SafeArrayMarshalDescriptor FromReader(IBinaryStreamReader reader)
        {
            var descriptor = new SafeArrayMarshalDescriptor();

            if (reader.CanRead((sizeof (byte))))
                descriptor.ElementType = (VariantType)reader.ReadByte();

            return descriptor;
        }

        public override NativeType NativeType
        {
            get { return NativeType.SafeArray; }
        }

        public VariantType ElementType
        {
            get;
            set;
        }

        public override uint GetPhysicalLength(MetadataBuffer buffer)
        {
            return 2 * sizeof (byte)
                + base.GetPhysicalLength(buffer);
        }

        public override void Prepare(MetadataBuffer buffer)
        {
        }

        public override void Write(MetadataBuffer buffer, IBinaryStreamWriter writer)
        {
            writer.WriteByte((byte)NativeType);
            writer.WriteByte((byte)ElementType);

            base.Write(buffer, writer);
        }
    }
}