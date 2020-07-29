using System;
using System.Threading.Tasks;
using Xunit;

namespace Common.UnitTest
{
    public class ServiceBaseTest
    {
        [Fact]
        public async Task InitialiseAsnc_Default_CallsInitialiseServiceAsync()
        {
            var mockService = new FakeService();
            mockService.StartTest();

            await mockService.InitialiseAsync();

            Assert.True(mockService.InitialiseServiceAsyncCalled);
        }

        [Fact]
        public async Task InitialiseAsnc_Initialised_DoesNotCallInitialiseServiceAsync()
        {
            var mockService = new FakeService();
            await mockService.InitialiseAsync();
            mockService.StartTest();

            await mockService.InitialiseAsync();

            Assert.False(mockService.InitialiseServiceAsyncCalled);
        }

        [Fact]
        public async Task InitialiseAsnc_Disposed_DoesNotCallInitialiseServiceAsync()
        {
            var mockService = new FakeService();
            await mockService.DisposeAsync();
            mockService.StartTest();

            await mockService.InitialiseAsync();

            Assert.False(mockService.InitialiseServiceAsyncCalled);
        }

        [Fact]
        public void Dispose_NotInitialised_CallsProtectedDisposeWithTrue()
        {
            var mockService = new FakeService();
            mockService.StartTest();

            mockService.Dispose();

            Assert.True(mockService.ProtectedDisposeCalled);
            Assert.True(mockService.ProtectedDisposeCallParameter);
        }

        [Fact]
        public async Task Dispose_Initialised_CallsProtectedDisposeWithTrue()
        {
            var mockService = new FakeService();
            await mockService.InitialiseAsync();
            mockService.StartTest();

            mockService.Dispose();

            Assert.True(mockService.ProtectedDisposeCalled);
            Assert.True(mockService.ProtectedDisposeCallParameter);
        }


        [Fact]
        public void Dispose_Disposed_CallsProtectedDisposeWithTrue()
        {
            var mockService = new FakeService();
            mockService.Dispose();
            mockService.StartTest();

            mockService.Dispose();

            Assert.True(mockService.ProtectedDisposeCalled);
            Assert.True(mockService.ProtectedDisposeCallParameter);
        }

        [Fact]
        public async Task DisposeAsync_NotInitialised_CallsDisposeServiceAsync()
        {
            var mockService = new FakeService();
            mockService.StartTest();

            await mockService.DisposeAsync();

            Assert.True(mockService.DisposeServiceAsyncCalled);
        }

        [Fact]
        public async Task DisposeAsync_Initialised_CallsDisposeServiceAsync()
        {
            var mockService = new FakeService();
            await mockService.InitialiseAsync();
            mockService.StartTest();

            await mockService.DisposeAsync();

            Assert.True(mockService.DisposeServiceAsyncCalled);
        }

        [Fact]
        public async Task DisposeAsync_Disposed_CallsDisposeServiceAsync()
        {
            var mockService = new FakeService();
            await mockService.DisposeAsync();
            mockService.StartTest();

            await mockService.DisposeAsync();

            Assert.True(mockService.DisposeServiceAsyncCalled);
        }

        [Fact]
        public async Task DisposeAsync_NotInitialised_CallsProtectedDisposeWithFalse()
        {
            var mockService = new FakeService();
            mockService.StartTest();

            await mockService.DisposeAsync();

            Assert.True(mockService.ProtectedDisposeCalled);
            Assert.False(mockService.ProtectedDisposeCallParameter);
        }

        [Fact]
        public async Task DisposeAsync_Initialised_CallsProtectedDisposeWithFalse()
        {
            var mockService = new FakeService();
            await mockService.InitialiseAsync();
            mockService.StartTest();

            await mockService.DisposeAsync();

            Assert.True(mockService.ProtectedDisposeCalled);
            Assert.False(mockService.ProtectedDisposeCallParameter);
        }

        [Fact]
        public async Task DisposeAsync_Disposed_CallsProtectedDisposeWithFalse()
        {
            var mockService = new FakeService();
            await mockService.DisposeAsync();
            mockService.StartTest();

            await mockService.DisposeAsync();

            Assert.True(mockService.ProtectedDisposeCalled);
            Assert.False(mockService.ProtectedDisposeCallParameter);
        }

        [Fact]
        public async Task CheckValidState_Initialised_DoesNotThrow()
        {
            var mockService = new FakeService();
            await mockService.InitialiseAsync();

            mockService.CheckValidState();
        }

        [Fact]
        public void CheckValidState_NotInitialised_ThrowsInvalidOperationException()
        {
            var mockService = new FakeService();

            Assert.Throws<InvalidOperationException>(() => mockService.CheckValidState());
        }

        [Fact]
        public void CheckValidState_Disposed_ThrowsObjectDisposedException()
        {
            var mockService = new FakeService();
            mockService.Dispose();

            Assert.Throws<InvalidOperationException>(() => mockService.CheckValidState());
        }

        private class FakeService : ServiceBase
        {
            public bool InitialiseServiceAsyncCalled { get; private set; }
            public bool DisposeServiceAsyncCalled { get; private set; }
            public bool ProtectedDisposeCalled { get; private set; }
            public bool ProtectedDisposeCallParameter { get; private set; }

            private bool testStarted = false;

            public void StartTest()
                => testStarted = true;

            protected override ValueTask InitialiseServiceAsync()
            {
                if (testStarted)
                {
                    InitialiseServiceAsyncCalled = true;
                }
                return base.InitialiseServiceAsync();
            }

            protected override ValueTask DisposeServiceAsync()
            {
                if (testStarted)
                {
                    DisposeServiceAsyncCalled = true;
                }
                return base.DisposeServiceAsync();
            }

            protected override void Dispose(bool disposing)
            {
                if (testStarted)
                {
                    ProtectedDisposeCalled = true;
                    ProtectedDisposeCallParameter = disposing;
                }
                base.Dispose(disposing);
            }

            public new void CheckValidState()
                => base.CheckValidState();
        }
    }
}
