/*
 * Copyright (c) 2017-2018 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * http://github.com/piranhacms/piranha
 * 
 */

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Piranha.AttributeBuilder.Tests
{
    public class AttributeBuilder : IDisposable
    {
        [PageType(Id = "Simple", Title = "Simple Page Type")]
        public class SimplePageType
        {
            [Region]
            public Extend.Fields.TextField Body { get; set; }
        }

        [PageType(Id = "Complex", Title = "Complex Page Type")]
        [PageTypeRoute(Title = "Default", Route = "/complex")]
        public class ComplexPageType
        {
            public class BodyRegion
            {
                [Field]
                public Extend.Fields.TextField Title { get; set; }
                [Field]
                public Extend.Fields.TextField Body { get; set; }
            }

            [Region(Title = "Intro", Min = 3, Max = 6)]
            public IList<Extend.Fields.TextField> Slider { get; set; }

            [Region(Title = "Main content")]
            public BodyRegion Content { get; set; }
        }

        [PostType(Id = "Simple", Title = "Simple Post Type")]
        public class SimplePostType
        {
            [Region]
            public Extend.Fields.TextField Body { get; set; }
        }

        [PostType(Id = "Complex", Title = "Complex Post Type")]
        [PostTypeRoute(Title = "Default", Route = "/complex")]
        public class ComplexPostType
        {
            public class BodyRegion
            {
                [Field]
                public Extend.Fields.TextField Title { get; set; }
                [Field]
                public Extend.Fields.TextField Body { get; set; }
            }

            [Region(Title = "Intro", Min = 3, Max = 6)]
            public IList<Extend.Fields.TextField> Slider { get; set; }

            [Region(Title = "Main content")]
            public BodyRegion Content { get; set; }
        }

        [BlockType(Id = "Simple", Title = "Simple Block Type")]
        public class SimpleBlockType
        {
            [Field]
            public Extend.Fields.TextField Body { get; set; }
        }

        [BlockType(Id = "Complex", Title = "Complex Block Type")]
        public class ComplexBlockType
        {
            [Field(Title = "Intro")]
            public Extend.Fields.TextField Slider { get; set; }

            [Field(Title = "Main content")]
            public Extend.Fields.HtmlField Content { get; set; }
        }

        public AttributeBuilder() {
            using (var api = new Api(GetDb(), null)) {
                App.Init(api);
            }
        }

        [Fact]
        public void AddSimple() {
            using (var api = new Api(GetDb(), null)) {
                var builder = new PageTypeBuilder(api)
                    .AddType(typeof(SimplePageType));
                builder.Build();

                var type = api.PageTypes.GetById("Simple");

                Assert.NotNull(type);
                Assert.Equal(1, type.Regions.Count);
                Assert.Equal("Body", type.Regions[0].Id);
                Assert.Equal(1, type.Regions[0].Fields.Count);
            }
        }

        [Fact]
        public void AddSimplePostType() {
            using (var api = new Api(GetDb(), null)) {
                var builder = new PostTypeBuilder(api)
                    .AddType(typeof(SimplePostType));
                builder.Build();

                var type = api.PostTypes.GetById("Simple");

                Assert.NotNull(type);
                Assert.Equal(1, type.Regions.Count);
                Assert.Equal("Body", type.Regions[0].Id);
                Assert.Equal(1, type.Regions[0].Fields.Count);
            }
        }

        [Fact]
        public void AddSimpleBlockType() {
            using (var api = new Api(GetDb(), null)) {
                var builder = new BlockTypeBuilder(api)
                    .AddType(typeof(SimpleBlockType));
                builder.Build();

                var type = api.BlockTypes.GetById("Simple");

                Assert.NotNull(type);
                Assert.Equal(1, type.Fields.Count);
                Assert.Equal("Body", type.Fields[0].Id);
                Assert.Equal(1, type.Fields.Count);
            }
        }

        [Fact]
        public void AddComplex() {
            using (var api = new Api(GetDb(), null)) {
                var builder = new PageTypeBuilder(api)
                    .AddType(typeof(ComplexPageType));
                builder.Build();

                var type = api.PageTypes.GetById("Complex");

                Assert.NotNull(type);
                Assert.Equal(2, type.Regions.Count);

                Assert.Equal("Slider", type.Regions[0].Id);
                Assert.Equal("Intro", type.Regions[0].Title);
                Assert.True(type.Regions[0].Collection);
                Assert.Equal(1, type.Regions[0].Fields.Count);

                Assert.Equal("Content", type.Regions[1].Id);
                Assert.Equal("Main content", type.Regions[1].Title);
                Assert.False(type.Regions[1].Collection);
                Assert.Equal(2, type.Regions[1].Fields.Count);

                Assert.Equal(1, type.Routes.Count);
                Assert.Equal("/complex", type.Routes[0]);
            }
        }

        [Fact]
        public void AddComplexPostType() {
            using (var api = new Api(GetDb(), null)) {
                var builder = new PostTypeBuilder(api)
                    .AddType(typeof(ComplexPostType));
                builder.Build();

                var type = api.PostTypes.GetById("Complex");

                Assert.NotNull(type);
                Assert.Equal(2, type.Regions.Count);

                Assert.Equal("Slider", type.Regions[0].Id);
                Assert.Equal("Intro", type.Regions[0].Title);
                Assert.True(type.Regions[0].Collection);
                Assert.Equal(1, type.Regions[0].Fields.Count);

                Assert.Equal("Content", type.Regions[1].Id);
                Assert.Equal("Main content", type.Regions[1].Title);
                Assert.False(type.Regions[1].Collection);
                Assert.Equal(2, type.Regions[1].Fields.Count);

                Assert.Equal(1, type.Routes.Count);
                Assert.Equal("/complex", type.Routes[0]);
            }
        }

        [Fact]
        public void AddComplexBlockType() {
            using (var api = new Api(GetDb(), null)) {
                var builder = new BlockTypeBuilder(api)
                    .AddType(typeof(ComplexBlockType));
                builder.Build();

                var type = api.BlockTypes.GetById("Complex");

                Assert.NotNull(type);
                Assert.Equal(2, type.Fields.Count);

                Assert.Equal("Slider", type.Fields[0].Id);
                Assert.Equal("Intro", type.Fields[0].Title);

                Assert.Equal("Content", type.Fields[1].Id);
                Assert.Equal("Main content", type.Fields[1].Title);
            }
        }

        [Fact]
        public void DeleteOrphans() {
            using (var api = new Api(GetDb(), null)) {
                var builder = new PageTypeBuilder(api)
                    .AddType(typeof(SimplePageType))
                    .AddType(typeof(ComplexPageType));
                builder.Build();

                Assert.Equal(2, api.PageTypes.GetAll().Count());

                builder = new PageTypeBuilder(api)
                    .AddType(typeof(SimplePageType));
                builder.DeleteOrphans();

                Assert.Single(api.PageTypes.GetAll());
            }
        }

        [Fact]
        public void DeleteOrphansPostType() {
            using (var api = new Api(GetDb(), null)) {
                var builder = new PostTypeBuilder(api)
                    .AddType(typeof(SimplePostType))
                    .AddType(typeof(ComplexPostType));
                builder.Build();

                Assert.Equal(2, api.PostTypes.GetAll().Count());

                builder = new PostTypeBuilder(api)
                    .AddType(typeof(SimplePostType));
                builder.DeleteOrphans();

                Assert.Single(api.PostTypes.GetAll());
            }
        }

        [Fact]
        public void DeleteOrphansBlockType() {
            using (var api = new Api(GetDb(), null)) {
                var builder = new BlockTypeBuilder(api)
                    .AddType(typeof(SimpleBlockType))
                    .AddType(typeof(ComplexBlockType));
                builder.Build();

                Assert.Equal(2, api.BlockTypes.GetAll().Count());

                builder = new BlockTypeBuilder(api)
                    .AddType(typeof(SimpleBlockType));
                builder.DeleteOrphans();

                Assert.Single(api.BlockTypes.GetAll());
            }
        }

        public void Dispose() {
            using (var api = new Api(GetDb(), null)) {
                var types = api.PageTypes.GetAll();

                foreach (var t in types)
                    api.PageTypes.Delete(t);

                var postTypes = api.PostTypes.GetAll();

                foreach (var t in postTypes)
                    api.PostTypes.Delete(t);
            }
        }

        /// <summary>
        /// Gets the test context.
        /// </summary>
        private IDb GetDb() {
            var builder = new DbContextOptionsBuilder<Db>();

            builder.UseSqlite("Filename=./piranha.tests.db");

            return new Db(builder.Options);
        }
    }
}
