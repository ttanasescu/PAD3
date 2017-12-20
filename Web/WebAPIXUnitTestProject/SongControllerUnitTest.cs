using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Web.Controllers;
using Web.Data;
using Web.Helpers;
using Web.Models;
using WebAPIXUnitTestProject.Helpers;
using Xunit;

namespace WebAPIXUnitTestProject
{
    public class SongsControllerUnitTest
    {
        #region SetUp

        private readonly SongsController _controller;
        private readonly IQueryable<Song> _testSongs;

        public SongsControllerUnitTest()
        {
            _testSongs = new List<Song>
            {
                new Song
                {
                    Duration = TimeSpan.FromSeconds(100),
                    Id = 1,
                    Lyrics = "lyrics",
                    Rating = 5,
                    Title = "Song1"
                },
                new Song
                {
                    Duration = TimeSpan.FromSeconds(200),
                    Id = 2,
                    Lyrics = "lyrics",
                    Rating = 5,
                    Title = "Song1"
                },
                new Song
                {
                    Duration = TimeSpan.FromSeconds(300),
                    Id = 3,
                    Lyrics = "lyrics",
                    Rating = 5,
                    Title = "Song3"
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Song>>();
            mockSet.As<IAsyncEnumerable<Song>>()
                .Setup(d => d.GetEnumerator())
                .Returns(new TestDbAsyncEnumerator<Song>(_testSongs.GetEnumerator()));

            mockSet.As<IQueryable<Song>>().Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Song>(_testSongs.Provider));
            mockSet.As<IQueryable<Song>>().Setup(m => m.Expression).Returns(_testSongs.Expression);
            mockSet.As<IQueryable<Song>>().Setup(m => m.ElementType).Returns(_testSongs.ElementType);
            mockSet.As<IQueryable<Song>>().Setup(m => m.GetEnumerator()).Returns(_testSongs.GetEnumerator());

            var dbContext = new Mock<DataContext>();
            dbContext.Setup(dc => dc.Songs).Returns(mockSet.Object);
            dbContext.Setup(dc => dc.Set<Song>()).Returns(mockSet.Object);

            _controller = new SongsController(dbContext.Object);
        }

        #endregion

        [Fact]
        public void CanGetAllSongs()
        {
            var result = _controller.GetSong(new SongFilter());
            Assert.Equal(result, _testSongs);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task CanGetSongByIdAsync(int id)
        {
            var result = await _controller.GetSong(id);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var song = okResult.Value.Should().BeAssignableTo<Song>().Subject;

            song.Id.Should().Be(id);
        }
        
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(10000)]
        public async Task CannotGetSong(int id)
        {
            var result = await _controller.GetSong(id);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CanPostSong()
        {
            var song = new Song
            {
                Duration = TimeSpan.FromSeconds(100),
                Id = 4,
                Lyrics = "lyrics",
                Rating = 5,
                Title = "Song11"
            };

            var result = await _controller.PostSong(song);

            var okResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var resultSong = okResult.Value.Should().BeAssignableTo<Song>().Subject;
            resultSong.Id.Should().Be(song.Id);
        }

        [Fact]
        public async Task CanDeleteSong()
        {
            var result = await _controller.DeleteSong(2);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CannotFindSongToDelete()
        {
            var result = await _controller.DeleteSong(20);

            result.Should().BeOfType<NotFoundResult>();
        }

        //[Fact]
        //public async Task CanPutSong()
        //{
        //    var song = new Song
        //    {
        //        Duration = TimeSpan.FromSeconds(100),
        //        Id = 1,
        //        Lyrics = "lyrics",
        //        Rating = 5,
        //        Title = "Song11"
        //    };

        //    var result = await _controller.PutSong(song.Id, song);

        //    result.Should().BeOfType<NoContentResult>();
        //}
    }
}