/*
 * Copyright (c) 2018 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * http://github.com/piranhacms/piranha
 * 
 */

using System;
using System.Collections.Generic;

namespace Piranha.Data
{
    public sealed class Block : Content<BlockField>, IModel, ICreated, IModified
    {
        /// <summary>
        /// Gets/sets the block type id.
        /// </summary>
        public string BlockTypeId { get; set; }

        /// <summary>
        /// Gets/sets the associated block type.
        /// </summary>
        public BlockType BlockType { get; set; }
    }
}
