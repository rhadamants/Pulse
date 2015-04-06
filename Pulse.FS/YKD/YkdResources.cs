﻿using System.IO;
using Pulse.Core;

namespace Pulse.FS
{
    public sealed class YkdResources : IStreamingContent
    {
        public YkdOffsets Offsets;
        public YkdResource[] Resources;

        public int Count
        {
            get { return Resources.Length; }
        }

        public YkdResource this[int index]
        {
            get { return Resources[index]; }
            set { Resources[index] = value; }
        }

        public void ReadFromStream(Stream stream)
        {
            Offsets = stream.ReadContent<YkdOffsets>();

            Resources = new YkdResource[Offsets.Count];
            for (int i = 0; i < Resources.Length; i++)
            {
                // Косытыль
                int resourceSize = (int)((i == Resources.Length - 1 ? stream.Length : Offsets[i + 1]) - Offsets[i]);

                YkdResource resource = Resources[i] = new YkdResource(resourceSize);
                
                stream.SetPosition(Offsets[i]);
                resource.ReadFromStream(stream);
            }

            if (!stream.IsEndOfStream())
                throw new InvalidDataException();
        }

        public void WriteToStream(Stream stream)
        {
            YkdOffsets.WriteToStream(stream, ref Offsets, ref Resources, b => b.Size);
            stream.WriteContent(Resources);
        }
    }
}