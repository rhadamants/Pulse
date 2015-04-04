﻿using System.Collections.Generic;

namespace Pulse.FS
{
    public sealed class WpdArchiveListing : List<WpdEntry>, IArchiveListing
    {
        public readonly ImgbArchiveAccessor Accessor;

        public WpdArchiveListing(ImgbArchiveAccessor accessor)
        {
            Accessor = accessor;
        }

        public WpdArchiveListing(ImgbArchiveAccessor accessor, int entriesCount)
            : base(entriesCount)
        {
            Accessor = accessor;
        }

        public string Name
        {
            get { return Accessor.Name; }
        }
    }
}