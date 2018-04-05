/*
 * Copyright (c) 2018 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * https://github.com/piranhacms/piranha.core
 * 
 */

using System;

namespace Piranha.Data
{
    public sealed class BlockField : ContentField, IModel
    {
        /// <summary>
        /// Gets/sets the block id.
        /// </summary>
        public Guid BlockId { get; set; }

        /// <summary>
        /// Gets/sets the block.
        /// </summary>
        public Block Block { get; set; }
    }
}
