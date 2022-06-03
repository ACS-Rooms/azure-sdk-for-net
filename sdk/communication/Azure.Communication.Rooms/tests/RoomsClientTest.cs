﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure.Communication.Rooms;
using Moq;
using NUnit.Framework;

namespace Azure.Communication.Rooms.Tests
{
    public class RoomsClientTest
    {
        [Test]
        public async Task CreateRoomAsyncShouldSucceed()
        {
            Mock<RoomsClient> mockRoomsClient = new Mock<RoomsClient>();
            var validFrom = new DateTime(2022, 05, 01, 00, 00, 00, DateTimeKind.Utc);
            var validUntil = validFrom.AddDays(1);

            List<RoomParticipant> createRoomParticipants = new List<RoomParticipant>();
            var mri1 = "8:acs:1b5cc06b-f352-4571-b1e6-d9b259b7c776_00000007-0464-274b-b274-5a3a0d000101";
            var mri2 = "8:acs:1b5cc06b-f352-4571-b1e6-d9b259b7c776_00000007-0464-274b-b274-5a3a0d000102";

            var communicationUser1 = new CommunicationUserIdentifier(mri1);
            var communicationUser2 = new CommunicationUserIdentifier(mri2);

            var participant1 = new RoomParticipant(communicationUser1, "Presenter");
            var participant2 = new RoomParticipant(communicationUser2, "Attendee");

            createRoomParticipants.Add(participant1);
            createRoomParticipants.Add(participant2);

            Response<RoomModel>? expectedRoomResult = default;
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            mockRoomsClient
                .Setup(roomsClient => roomsClient.CreateRoomAsync(validFrom, validUntil, createRoomParticipants, cancellationToken))
                .ReturnsAsync(expectedRoomResult);

            Response<RoomModel> actualResponse = await mockRoomsClient.Object.CreateRoomAsync(validFrom, validUntil, createRoomParticipants, cancellationToken);

            mockRoomsClient.Verify(roomsClient => roomsClient.CreateRoomAsync(validFrom, validUntil, createRoomParticipants, cancellationToken), Times.Once());
            Assert.AreEqual(expectedRoomResult, actualResponse);
        }

        [Test]
        public void CreateRoomShouldSucceed()
        {
            Mock<RoomsClient> mockRoomsClient = new Mock<RoomsClient>();
            var validFrom = new DateTime(2022, 07, 01, 00, 00, 00, DateTimeKind.Utc);
            var validUntil = validFrom.AddDays(1);
            List<RoomParticipant> createRoomParticipants = new List<RoomParticipant>();
            string communicationUser1 = "mockAcsUserIdentityString1";
            string communicationUser2 = "mockAcsUserIdentityString2";

            createRoomParticipants.Add(new RoomParticipant(new CommunicationUserIdentifier(communicationUser1), "Presenter"));
            createRoomParticipants.Add(new RoomParticipant(new CommunicationUserIdentifier(communicationUser2), "Attendee"));
            Response<RoomModel>? expectedRoomResult = new Mock<Response<RoomModel>>().Object;
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            mockRoomsClient
                .Setup(roomsClient => roomsClient.CreateRoom(validFrom, validUntil, createRoomParticipants, cancellationToken))
                .Returns(expectedRoomResult);

            Response<RoomModel> actualResponse = mockRoomsClient.Object.CreateRoom(validFrom, validUntil, createRoomParticipants, cancellationToken);

            mockRoomsClient.Verify(roomsClient => roomsClient.CreateRoom(validFrom, validUntil, createRoomParticipants, cancellationToken), Times.Once());
            Assert.AreEqual(expectedRoomResult, actualResponse);
        }

        [Test]
        public async Task UpdateRoomAsyncShouldSucceed()
        {
            var roomId = "123";
            Mock<RoomsClient> mockRoomsClient = new Mock<RoomsClient>();
            var validFrom = new DateTime(2022, 05, 01, 00, 00, 00, DateTimeKind.Utc);
            var validUntil = validFrom.AddDays(1);
            Response<RoomModel>? expectedRoomResult = default;
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            mockRoomsClient
            .Setup(roomsClient => roomsClient.UpdateRoomAsync(roomId, validFrom, validUntil, default, cancellationToken))
            .ReturnsAsync(expectedRoomResult);

            Response<RoomModel> actualResponse = await mockRoomsClient.Object.UpdateRoomAsync(roomId, validFrom, validUntil, default, cancellationToken);

            mockRoomsClient.Verify(roomsClient => roomsClient.UpdateRoomAsync(roomId, validFrom, validUntil, default, cancellationToken), Times.Once());
            Assert.AreEqual(expectedRoomResult, actualResponse);
        }

        [Test]
        public void UpdateRoomShouldSucceed()
        {
            Mock<RoomsClient> mockRoomsClient = new Mock<RoomsClient>();
            string roomId = "123";
            var validFrom = new DateTime(2022, 05, 01, 00, 00, 00, DateTimeKind.Utc);
            var validUntil = validFrom.AddDays(1);

            Response<RoomModel>? expectedRoomResult = new Mock<Response<RoomModel>>().Object;
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            mockRoomsClient
            .Setup(roomsClient => roomsClient.UpdateRoom(roomId, validFrom, validUntil, default, cancellationToken))
            .Returns(expectedRoomResult);

            Response<RoomModel> actualResponse = mockRoomsClient.Object.UpdateRoom(roomId, validFrom, validUntil, default, cancellationToken);

            mockRoomsClient.Verify(roomsClient => roomsClient.UpdateRoom(roomId, validFrom, validUntil, default, cancellationToken), Times.Once());
            Assert.AreEqual(expectedRoomResult, actualResponse);
        }

        [Test]
        public async Task GetRoomAsyncShouldSucceed()
        {
            var roomId = "123";
            Mock<RoomsClient> mockRoomsClient = new Mock<RoomsClient>();
            Response<RoomModel>? expectedRoomResult = default;
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            mockRoomsClient
            .Setup(roomsClient => roomsClient.GetRoomAsync(roomId, cancellationToken))
            .ReturnsAsync(expectedRoomResult);

            Response<RoomModel> actualResponse = await mockRoomsClient.Object.GetRoomAsync(roomId, cancellationToken);

            mockRoomsClient.Verify(roomsClient => roomsClient.GetRoomAsync(roomId, cancellationToken), Times.Once());
            Assert.AreEqual(expectedRoomResult, actualResponse);
        }

        [Test]
        public void GetRoomShouldSucceed()
        {
            Mock<RoomsClient> mockRoomsClient = new Mock<RoomsClient>();
            string roomId = "123";
            Response<RoomModel>? expectedRoomResult = new Mock<Response<RoomModel>>().Object;
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            mockRoomsClient
            .Setup(roomsClient => roomsClient.GetRoom(roomId, cancellationToken))
            .Returns(expectedRoomResult);

            Response<RoomModel> actualResponse = mockRoomsClient.Object.GetRoom(roomId, cancellationToken);

            mockRoomsClient.Verify(roomsClient => roomsClient.GetRoom(roomId, cancellationToken), Times.Once());
            Assert.AreEqual(expectedRoomResult, actualResponse);
        }

        [Test]
        public async Task DeleteRoomAsyncShouldSucceed()
        {
            var roomId = "123";
            Mock<RoomsClient> mockRoomsClient = new Mock<RoomsClient>();
            Response? expectedRoomResult = default;
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            mockRoomsClient
            .Setup(roomsClient => roomsClient.DeleteRoomAsync(roomId, cancellationToken))
            .ReturnsAsync(expectedRoomResult);

            Response actualResponse = await mockRoomsClient.Object.DeleteRoomAsync(roomId, cancellationToken);

            mockRoomsClient.Verify(roomsClient => roomsClient.DeleteRoomAsync(roomId, cancellationToken), Times.Once());
            Assert.AreEqual(expectedRoomResult, actualResponse);
        }

        [Test]
        public void DeleteRoomShouldSucceed()
        {
            Mock<RoomsClient> mockRoomsClient = new Mock<RoomsClient>();
            string roomId = "123";
            Response? expectedRoomResult = new Mock<Response>().Object;
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            mockRoomsClient
            .Setup(roomsClient => roomsClient.DeleteRoom(roomId, cancellationToken))
            .Returns(expectedRoomResult);

            Response actualResponse = mockRoomsClient.Object.DeleteRoom(roomId, cancellationToken);

            mockRoomsClient.Verify(roomsClient => roomsClient.DeleteRoom(roomId, cancellationToken), Times.Once());
            Assert.AreEqual(expectedRoomResult, actualResponse);
        }

        [Test]
        public async Task AddParticipantAsyncShouldSucceed()
        {
            Mock<RoomsClient> mockRoomsClient = new Mock<RoomsClient>();

            List<RoomParticipant> participants = new List<RoomParticipant>();
            var mri1 = "8:acs:1b5cc06b-f352-4571-b1e6-d9b259b7c776_00000007-0464-274b-b274-5a3a0d000101";
            var mri2 = "8:acs:1b5cc06b-f352-4571-b1e6-d9b259b7c776_00000007-0464-274b-b274-5a3a0d000102";

            var communicationUser1 = new CommunicationUserIdentifier(mri1);
            var communicationUser2 = new CommunicationUserIdentifier(mri2);

            var participant1 = new RoomParticipant(communicationUser1, "Presenter");
            var participant2 = new RoomParticipant(communicationUser2, "Attendee");

            participants.Add(participant1);
            participants.Add(participant2);

            string roomId = "123";

            Response<RoomModel>? expectedRoomResult = default;
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            mockRoomsClient
                .Setup(roomsClient => roomsClient.AddParticipantsAsync(roomId, participants, cancellationToken))
                .ReturnsAsync(expectedRoomResult);

            Response<RoomModel> actualResponse = await mockRoomsClient.Object.AddParticipantsAsync(roomId, participants, cancellationToken);

            mockRoomsClient.Verify(roomsClient => roomsClient.AddParticipantsAsync(roomId, participants, cancellationToken), Times.Once());
            Assert.AreEqual(expectedRoomResult, actualResponse);
        }

        [Test]
        public void AddParticipantsShouldSucceed()
        {
            Mock<RoomsClient> mockRoomsClient = new Mock<RoomsClient>();
            string roomId = "123";
            List<RoomParticipant> communicationUsers = new List<RoomParticipant>();
            string communicationUser1 = "mockAcsUserIdentityString1";
            string communicationUser2 = "mockAcsUserIdentityString2";
            string communicationUser3 = "mockAcsUserIdentityString3";

            communicationUsers.Add(new RoomParticipant(new CommunicationUserIdentifier(communicationUser1), "Presenter"));
            communicationUsers.Add(new RoomParticipant(new CommunicationUserIdentifier(communicationUser2), "Attendee"));
            communicationUsers.Add(new RoomParticipant(new CommunicationUserIdentifier(communicationUser3), "Organizer"));

            Response<RoomModel>? expectedRoomResult = new Mock<Response<RoomModel>>().Object;
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            mockRoomsClient
            .Setup(roomsClient => roomsClient.AddParticipants(roomId, communicationUsers, cancellationToken))
            .Returns(expectedRoomResult);

            Response<RoomModel> actualResponse = mockRoomsClient.Object.AddParticipants(roomId, communicationUsers, cancellationToken);

            mockRoomsClient.Verify(roomsClient => roomsClient.AddParticipants(roomId, communicationUsers, cancellationToken), Times.Once());
            Assert.AreEqual(expectedRoomResult, actualResponse);
        }

        [Test]
        public void UpdateParticipantsShouldSucceed()
        {
            Mock<RoomsClient> mockRoomsClient = new Mock<RoomsClient>();
            string roomId = "123";
            List<RoomParticipant> communicationUsers = new List<RoomParticipant>();
            string communicationUser1 = "mockAcsUserIdentityString1";
            string communicationUser2 = "mockAcsUserIdentityString2";
            string communicationUser3 = "mockAcsUserIdentityString3";

            communicationUsers.Add(new RoomParticipant(new CommunicationUserIdentifier(communicationUser1), "Presenter"));
            communicationUsers.Add(new RoomParticipant(new CommunicationUserIdentifier(communicationUser2), "Attendee"));
            communicationUsers.Add(new RoomParticipant(new CommunicationUserIdentifier(communicationUser3), "Organizer"));

            Response<RoomModel>? expectedRoomResult = new Mock<Response<RoomModel>>().Object;
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            mockRoomsClient
            .Setup(roomsClient => roomsClient.UpdateParticipants(roomId, communicationUsers, cancellationToken))
            .Returns(expectedRoomResult);

            Response<RoomModel> actualResponse = mockRoomsClient.Object.UpdateParticipants(roomId, communicationUsers, cancellationToken);

            mockRoomsClient.Verify(roomsClient => roomsClient.UpdateParticipants(roomId, communicationUsers, cancellationToken), Times.Once());
            Assert.AreEqual(expectedRoomResult, actualResponse);
        }

        [Test]
        public void RemoveParticipantsShouldSucceed()
        {
            Mock<RoomsClient> mockRoomsClient = new Mock<RoomsClient>();
            string roomId = "123";

            List<RoomParticipant> communicationUsers = new List<RoomParticipant>();
            communicationUsers.Add(new RoomParticipant(new CommunicationUserIdentifier("mockAcsUserIdentityString1")));
            communicationUsers.Add(new RoomParticipant(new CommunicationUserIdentifier("mockAcsUserIdentityString2")));
            communicationUsers.Add(new RoomParticipant(new CommunicationUserIdentifier("mockAcsUserIdentityString3")));

            Response<RoomModel>? expectedRoomResult = new Mock<Response<RoomModel>>().Object;
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            mockRoomsClient
            .Setup(roomsClient => roomsClient.RemoveParticipants(roomId, communicationUsers, cancellationToken))
            .Returns(expectedRoomResult);

            Response<RoomModel> actualResponse = mockRoomsClient.Object.RemoveParticipants(roomId, communicationUsers, cancellationToken);

            mockRoomsClient.Verify(roomsClient => roomsClient.RemoveParticipants(roomId, communicationUsers, cancellationToken), Times.Once());
            Assert.AreEqual(expectedRoomResult, actualResponse);
        }

        [Test]
        public void GetParticipantsShouldSucceed()
        {
            string roomId = "123";

            Response<ParticipantsCollection>? expectedRoomResult = new Mock<Response<ParticipantsCollection>>().Object;
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            Mock<RoomsClient> mockRoomsClient = new Mock<RoomsClient>();
            mockRoomsClient
            .Setup(roomsClient => roomsClient.GetParticipants(roomId, cancellationToken))
            .Returns(expectedRoomResult);

            Response<ParticipantsCollection> actualResponse = mockRoomsClient.Object.GetParticipants(roomId, cancellationToken);

            mockRoomsClient.Verify(roomsClient => roomsClient.GetParticipants(roomId, cancellationToken), Times.Once());
            Assert.AreEqual(expectedRoomResult, actualResponse);
        }
    }
}
