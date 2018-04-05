/*
 * Copyright (c) 2018 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * http://github.com/piranhacms/piranha
 * 
 */

using Piranha.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Piranha.Tests.Repositories
{
    [Collection("Integration tests")]
    public class BlockTypesCached : BlockTypes
    {
        protected override void Init() {
            cache = new Cache.MemCache();

            base.Init();
        }
    }

    [Collection("Integration tests")]
    public class BlockTypes : BaseTests
    {
        #region Members
        protected ICache cache;
        private List<BlockType> blockTypes = new List<BlockType>() {
            new BlockType() {
                Id = "MyFirstType",
                Fields = new List<FieldType>() {
                    new FieldType() {
                        Id = "Body",
                        Type = "Html"
                    }
                }
            },
            new BlockType() {
                Id = "MySecondType",
                Fields = new List<FieldType>() {
                    new FieldType() {
                        Id = "Body",
                        Type = "Text"
                    }
                }
            },
            new BlockType() {
                Id = "MyThirdType",
                Fields = new List<FieldType>() {
                    new FieldType() {
                        Id = "Body",
                        Type = "Image"
                    }
                }
            },
            new BlockType() {
                Id = "MyFourthType",
                Fields = new List<FieldType>() {
                    new FieldType() {
                        Id = "Body",
                        Type = "String"
                    }
                }
            },
            new BlockType() {
                Id = "MyFifthType",
                Fields = new List<FieldType>() {
                    new FieldType() {
                        Id = "Body",
                        Type = "Text"
                    }
                }
            }
        };
        #endregion

        protected override void Init() {
            using (var api = new Api(GetDb(), storage, cache)) {
                api.BlockTypes.Save(blockTypes[0]);
                api.BlockTypes.Save(blockTypes[3]);
                api.BlockTypes.Save(blockTypes[4]);
            }
        }

        protected override void Cleanup() {
            using (var api = new Api(GetDb(), storage, cache)) {
                var blockTypes = api.BlockTypes.GetAll();

                foreach (var b in blockTypes)
                    api.BlockTypes.Delete(b);
            }
        }

        [Fact]
        public void IsCached() {
            using (var api = new Api(GetDb(), storage, cache)) {
                Assert.Equal(this.GetType() == typeof(BlockTypesCached), api.IsCached);
            }
        }        

        [Fact]
        public void Add() {
            using (var api = new Api(GetDb(), storage, cache)) {
                api.BlockTypes.Save(blockTypes[1]);
            }
        }

        [Fact]
        public void GetAll() {
            using (var api = new Api(GetDb(), storage, cache)) {
                var models = api.BlockTypes.GetAll();

                Assert.NotNull(models);
                Assert.NotEmpty(models);
            }
        }

        [Fact]
        public void GetNoneById() {
            using (var api = new Api(GetDb(), storage, cache)) {
                var none = api.BlockTypes.GetById("none-existing-type");

                Assert.Null(none);
            }
        }


        [Fact]
        public void GetById() {
            using (var api = new Api(GetDb(), storage, cache)) {
                var model = api.BlockTypes.GetById(blockTypes[0].Id);

                Assert.NotNull(model);
                Assert.Equal(blockTypes[0].Fields[0].Id, model.Fields[0].Id);
            }
        }

        [Fact]
        public void Update() {
            using (var api = new Api(GetDb(), storage, cache)) {
                var model = api.BlockTypes.GetById(blockTypes[0].Id);

                Assert.Null(model.Title);

                model.Title = "Updated";

                api.BlockTypes.Save(model);
            }
        }

        [Fact]
        public void Delete() {
            using (var api = new Api(GetDb(), storage, cache)) {
                var model = api.BlockTypes.GetById(blockTypes[3].Id);

                Assert.NotNull(model);

                api.BlockTypes.Delete(model);
            }
        }

        [Fact]
        public void DeleteById() {
            using (var api = new Api(GetDb(), storage, cache)) {
                var model = api.BlockTypes.GetById(blockTypes[4].Id);

                Assert.NotNull(model);

                api.BlockTypes.Delete(model.Id);
            }
        }
    }
}
