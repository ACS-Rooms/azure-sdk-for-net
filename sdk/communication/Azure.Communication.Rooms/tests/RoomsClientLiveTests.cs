// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#region Snippet:Azure_Communication_Rooms_Tests_UsingStatements
using System;
using System.Collections.Generic;
using System.Linq;
//@@ using Azure.Communication.Rooms
#endregion Snippet:Azure_Communication_Rooms_Tests_UsingStatements
using System.Threading.Tasks;
using Azure.Communication.Identity;
using Azure.Communication.Rooms;
using NUnit.Framework;

namespace Azure.Communication.Rooms.Tests
{
    public class RoomsClientLiveTests : RoomsClientLiveTestBase
    {
        public RoomsClientLiveTests(bool isAsync) : base(isAsync)
        {
        }

        [Test]
        public async Task AcsRoomRequestLiveWithoutParticipantsTest()
        {
            RoomsClient roomsClient = CreateInstrumentedRoomsClient(RoomsClientOptions.ServiceVersion.V2022_02_01_Preview);
            CommunicationIdentityClient communicationIdentityClient = CreateInstrumentedCommunicationIdentityClient();

            var validFrom = DateTime.Now;
            var validUntil = validFrom.AddDays(1);

            try
            {
                // Create Room
                Response<RoomModel> createRoomResponse = await roomsClient.CreateRoomAsync(validFrom, validUntil);
                RoomModel createCommunicationRoom = createRoomResponse.Value;
                List<RoomParticipant> createRoomParticipantsResult = createCommunicationRoom.Participants.ToList();
                Assert.IsFalse(string.IsNullOrWhiteSpace(createCommunicationRoom.Id));
                Assert.AreEqual(0, createRoomParticipantsResult.Count, "Expected CreateRoom participants count to be 0");

                var createdRoomId = createCommunicationRoom.Id;

                // Get Room
                Response<RoomModel> getRoomResponse = await roomsClient.GetRoomAsync(createdRoomId);
                RoomModel getCommunicationRoom = getRoomResponse.Value;
                List<RoomParticipant> getRoomParticipantsResult = getCommunicationRoom.Participants.ToList();
                Assert.AreEqual(createdRoomId, getCommunicationRoom.Id);
                Assert.AreEqual(0, getRoomParticipantsResult.Count, "Expected GetRoom participants count to be 0");

                // Update Room
                List<RoomParticipant> updateRoomParticipants = new List<RoomParticipant>();

                Response<RoomModel> updateRoomResponse = await roomsClient.UpdateRoomAsync(createdRoomId, validFrom.AddDays(1), validUntil.AddDays(2));
                RoomModel updateCommunicationRoom = updateRoomResponse.Value;
                List<RoomParticipant> updateRoomParticipantsResult = updateCommunicationRoom.Participants.ToList();
                Assert.AreEqual(createdRoomId, updateCommunicationRoom.Id);
                Assert.AreEqual(0, updateRoomParticipantsResult.Count, "Expected UpdateRoom participants count to be 0");
                Assert.AreEqual(updateCommunicationRoom.ValidUntil, updateCommunicationRoom.ValidUntil);

                // Delete Room
                Response deleteRoomResponse = await roomsClient.DeleteRoomAsync(createdRoomId);
                Assert.AreEqual(204, deleteRoomResponse.Status);
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail($"Unexpected error: {ex}");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected error: {ex}");
            }
        }

        [Test]
        public async Task AcsRoomRequestLiveTest()
        {
            RoomsClient roomsClient = CreateInstrumentedRoomsClient(RoomsClientOptions.ServiceVersion.V2022_02_01_Preview);
            CommunicationIdentityClient communicationIdentityClient = CreateInstrumentedCommunicationIdentityClient();

            var communicationUser1 = communicationIdentityClient.CreateUserAsync().Result.Value.Id;
            var communicationUser2 = communicationIdentityClient.CreateUserAsync().Result.Value.Id;
            var communicationUser3 = communicationIdentityClient.CreateUserAsync().Result.Value.Id;

            var validFrom = DateTime.Now;
            var validUntil = validFrom.AddDays(1);

            try
            {
                List<RoomParticipant> createRoomParticipants = new List<RoomParticipant>();
                RoomParticipant participant1 = new RoomParticipant(new CommunicationUserIdentifier(communicationUser1), "Presenter");
                RoomParticipant participant2 = new RoomParticipant(new CommunicationUserIdentifier(communicationUser2));
                createRoomParticipants.Add(participant1);
                createRoomParticipants.Add(participant2);

                // Create Room
                Response<RoomModel> createRoomResponse = await roomsClient.CreateRoomAsync(validFrom, validUntil, createRoomParticipants);
                RoomModel createCommunicationRoom = createRoomResponse.Value;
                List<RoomParticipant> createRoomParticipantsResult = createCommunicationRoom.Participants.ToList();
                Assert.IsFalse(string.IsNullOrWhiteSpace(createCommunicationRoom.Id));
                Assert.AreEqual(2, createRoomParticipantsResult.Count, "Expected CreateRoom participants count to be 2");
                Assert.IsTrue(createRoomParticipantsResult.Any(x => x.CommunicationIdentifier == participant1.CommunicationIdentifier), "Expected CreateRoom to contain user1");
                Assert.IsTrue(createRoomParticipantsResult.Any(x => x.CommunicationIdentifier == participant2.CommunicationIdentifier), "Expected CreateRoom to contain user2");
                Assert.IsTrue(createRoomParticipantsResult.Any(x => x.Role == participant1.Role), "Expected CreateRoom to contain role1");
                Assert.IsTrue(createRoomParticipantsResult.Any(x => x.Role == "Attendee"), "Expected CreateRoom to contain role2");

                var createdRoomId = createCommunicationRoom.Id;

                // Get Room
                Response<RoomModel> getRoomResponse = await roomsClient.GetRoomAsync(createdRoomId);
                RoomModel getCommunicationRoom = getRoomResponse.Value;
                List<RoomParticipant> getRoomParticipantsResult = getCommunicationRoom.Participants.ToList();
                Assert.AreEqual(createdRoomId, getCommunicationRoom.Id);
                Assert.AreEqual(2, getRoomParticipantsResult.Count, "Expected GetRoom participants count to be 2");
                Assert.IsTrue(getRoomParticipantsResult.Any(x => x.CommunicationIdentifier == participant1.CommunicationIdentifier), "Expected GetRoom to contain user1");
                Assert.IsTrue(getRoomParticipantsResult.Any(x => x.CommunicationIdentifier == participant2.CommunicationIdentifier), "Expected GetRoom to contain user2");
                Assert.IsTrue(getRoomParticipantsResult.Any(x => x.Role == participant1.Role), "Expected GetRoom to contain role1");
                Assert.IsTrue(getRoomParticipantsResult.Any(x => x.Role == participant2.Role), "Expected GetRoom to contain role2");

                // Update Room
                List<RoomParticipant> updateRoomParticipants = new List<RoomParticipant>();
                RoomParticipant participant3 = new RoomParticipant(new CommunicationUserIdentifier(communicationUser3), "Presenter");
                createRoomParticipants.Add(participant3);

                Response<RoomModel> updateRoomResponse = await roomsClient.UpdateRoomAsync(createdRoomId, validFrom, validUntil);
                RoomModel updateCommunicationRoom = updateRoomResponse.Value;
                List<RoomParticipant> updateRoomParticipantsResult = updateCommunicationRoom.Participants.ToList();
                Assert.AreEqual(createdRoomId, updateCommunicationRoom.Id);
                Assert.AreEqual(2, updateRoomParticipantsResult.Count, "Expected UpdateRoom participants count to be 2");
                Assert.IsTrue(updateRoomParticipantsResult.Any(x => x.CommunicationIdentifier == participant3.CommunicationIdentifier), "Expected UpdateRoom to contain user3");
                Assert.IsFalse(updateRoomParticipantsResult.Any(x => x.CommunicationIdentifier == participant1.CommunicationIdentifier), "Expected UpdateRoom to not contain user1");
                Assert.IsFalse(updateRoomParticipantsResult.Any(x => x.CommunicationIdentifier == participant2.CommunicationIdentifier), "Expected UpdateRoom to not contain user2");
                Assert.AreEqual(updateCommunicationRoom.ValidUntil, updateCommunicationRoom.ValidUntil);

                // Delete Room
                Response deleteRoomResponse = await roomsClient.DeleteRoomAsync(createdRoomId);
                Assert.AreEqual(204, deleteRoomResponse.Status);
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail($"Unexpected error: {ex}");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected error: {ex}");
            }
        }

        [Test]
        public async Task RoomParticipantsMethodLiveTest()
        {
            CommunicationIdentityClient communicationIdentityClient = CreateInstrumentedCommunicationIdentityClient();
            var communicationUser1 = communicationIdentityClient.CreateUserAsync().Result.Value.Id;
            var communicationUser2 = communicationIdentityClient.CreateUserAsync().Result.Value.Id;
            var communicationUser4 = communicationIdentityClient.CreateUserAsync().Result.Value.Id;
            var communicationUser5 = communicationIdentityClient.CreateUserAsync().Result.Value.Id;
            var communicationUser6 = communicationIdentityClient.CreateUserAsync().Result.Value.Id;

            RoomsClient roomsClient = CreateInstrumentedRoomsClient(RoomsClientOptions.ServiceVersion.V2022_02_01_Preview);

            RoomParticipant participant1 = new RoomParticipant(new CommunicationUserIdentifier(communicationUser1), "Presenter");
            RoomParticipant participant2 = new RoomParticipant(new CommunicationUserIdentifier(communicationUser2), "Organizer");
            RoomParticipant participant4 = new RoomParticipant(new CommunicationUserIdentifier(communicationUser4), "Consumer");
            RoomParticipant participant5 = new RoomParticipant(new CommunicationUserIdentifier(communicationUser5), "Attendee");
            RoomParticipant participant6 = new RoomParticipant(new CommunicationUserIdentifier(communicationUser6), "Attendee");

            var validFrom = DateTime.Now;
            var validUntil = validFrom.AddDays(1);

            try
            {
                List<RoomParticipant> createRoomParticipants = new List<RoomParticipant>();
                createRoomParticipants.Add(participant1);
                createRoomParticipants.Add(participant2);

                Response<RoomModel> createRoomResponse = await roomsClient.CreateRoomAsync(validFrom, validUntil, createRoomParticipants);
                RoomModel createCommunicationRoom = createRoomResponse.Value;
                List<RoomParticipant> createRoomParticipantsResult = createCommunicationRoom.Participants.ToList();

                Assert.IsFalse(string.IsNullOrWhiteSpace(createCommunicationRoom.Id));
                Assert.AreEqual(2, createRoomParticipantsResult.Count, "Expected CreateRoom participants count to be 2");
                Assert.IsTrue(createRoomParticipantsResult.Any(x => x.CommunicationIdentifier == participant1.CommunicationIdentifier), "Expected CreateRoom to contain user1");
                Assert.IsTrue(createRoomParticipantsResult.Any(x => x.CommunicationIdentifier == participant2.CommunicationIdentifier), "Expected CreateRoom to contain user2");
                Assert.IsTrue(createRoomParticipantsResult.Any(x => x.Role == participant1.Role), "Expected CreateRoom to contain role1");
                Assert.IsTrue(createRoomParticipantsResult.Any(x => x.Role == participant2.Role), "Expected CreateRoom to contain role2");

                var createdRoomId = createCommunicationRoom.Id;

                List<RoomParticipant> toAddCommunicationUsers = new List<RoomParticipant>();
                toAddCommunicationUsers.Add(participant4);
                toAddCommunicationUsers.Add(participant5);

                Response<RoomModel> addParticipantsResponse = await roomsClient.AddParticipantsAsync(createdRoomId, toAddCommunicationUsers);
                RoomModel addParticipantsRoom = addParticipantsResponse.Value;
                List<RoomParticipant> addRoomParticipantsResult = addParticipantsRoom.Participants.ToList();
                Assert.AreEqual(createdRoomId, addParticipantsRoom.Id);
                Assert.AreEqual(4, addRoomParticipantsResult.Count, "Expected Room participants count to be 4");
                Assert.IsTrue(addRoomParticipantsResult.Any(x => x.CommunicationIdentifier == participant4.CommunicationIdentifier), "Expected AddParticipants to contain user4");
                Assert.IsTrue(addRoomParticipantsResult.Any(x => x.CommunicationIdentifier == participant5.CommunicationIdentifier), "Expected AddParticipants to contain user5");
                Assert.IsTrue(addRoomParticipantsResult.Any(x => x.Role == participant4.Role), "Expected AddParticipants to contain role4");
                Assert.IsTrue(addRoomParticipantsResult.Any(x => x.Role == participant5.Role), "Expected AddParticipants to contain rol55");

                List<CommunicationIdentifier> toRemoveCommunicationUsers = new List<CommunicationIdentifier>();
                toRemoveCommunicationUsers.Add(new CommunicationUserIdentifier(communicationUser5));
                toRemoveCommunicationUsers.Add(new CommunicationUserIdentifier(communicationUser6));
                Response<RoomModel> removeParticipantsResponse = await roomsClient.RemoveParticipantsAsync(createdRoomId, toRemoveCommunicationUsers);
                RoomModel removeParticipantsRoom = removeParticipantsResponse.Value;
                Assert.AreEqual(createdRoomId, removeParticipantsRoom.Id);

                Response deleteRoomResponse = await roomsClient.DeleteRoomAsync(createdRoomId);
                Assert.AreEqual(204, deleteRoomResponse.Status);
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail($"Unexpected error: {ex}");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected error: {ex}");
            }
        }

        [Test]
        public async Task UpdateParticipantRoleMethodLiveTest()
        {
            CommunicationIdentityClient communicationIdentityClient = CreateInstrumentedCommunicationIdentityClient();
            var communicationUser1 = communicationIdentityClient.CreateUserAsync().Result.Value.Id;
            var communicationUser2 = communicationIdentityClient.CreateUserAsync().Result.Value.Id;

            RoomsClient roomsClient = CreateInstrumentedRoomsClient(RoomsClientOptions.ServiceVersion.V2022_02_01_Preview);

            RoomParticipant participant1 = new RoomParticipant(new CommunicationUserIdentifier(communicationUser1), "Presenter");
            RoomParticipant participant2 = new RoomParticipant(new CommunicationUserIdentifier(communicationUser2), "Organizer");
            RoomParticipant participant4 = new RoomParticipant(new CommunicationUserIdentifier(communicationUser1), "Consumer");
            RoomParticipant participant5 = new RoomParticipant(new CommunicationUserIdentifier(communicationUser2), "Attendee");

            var validFrom = DateTime.Now;
            var validUntil = validFrom.AddDays(1);

            try
            {
                List<RoomParticipant> createRoomParticipants = new List<RoomParticipant>();
                createRoomParticipants.Add(participant1);
                createRoomParticipants.Add(participant2);

                Response<RoomModel> createRoomResponse = await roomsClient.CreateRoomAsync(validFrom, validUntil, createRoomParticipants);
                RoomModel createCommunicationRoom = createRoomResponse.Value;
                List<RoomParticipant> createRoomParticipantsResult = createCommunicationRoom.Participants.ToList();

                Assert.IsFalse(string.IsNullOrWhiteSpace(createCommunicationRoom.Id));
                Assert.AreEqual(2, createRoomParticipantsResult.Count, "Expected CreateRoom participants count to be 2");
                Assert.IsTrue(createRoomParticipantsResult.Any(x => x.CommunicationIdentifier == participant1.CommunicationIdentifier), "Expected CreateRoom to contain user1");
                Assert.IsTrue(createRoomParticipantsResult.Any(x => x.CommunicationIdentifier == participant2.CommunicationIdentifier), "Expected CreateRoom to contain user2");
                Assert.IsTrue(createRoomParticipantsResult.Any(x => x.Role == participant1.Role), "Expected CreateRoom to contain role1");
                Assert.IsTrue(createRoomParticipantsResult.Any(x => x.Role == participant2.Role), "Expected CreateRoom to contain role2");

                var createdRoomId = createCommunicationRoom.Id;

                List<RoomParticipant> toUpdateCommunicationUsers = new List<RoomParticipant>();
                toUpdateCommunicationUsers.Add(participant4);
                toUpdateCommunicationUsers.Add(participant5);

                Response<RoomModel> updateParticipantsResponse = await roomsClient.UpdateParticipantsAsync(createdRoomId, toUpdateCommunicationUsers);
                RoomModel updateParticipantsRoom = updateParticipantsResponse.Value;
                List<RoomParticipant> updateRoomParticipantsResult = updateParticipantsRoom.Participants.ToList();
                Assert.AreEqual(createdRoomId, updateParticipantsRoom.Id);
                Assert.AreEqual(2, updateRoomParticipantsResult.Count, 2);
                Assert.IsTrue(updateRoomParticipantsResult.Any(x => x.CommunicationIdentifier == participant4.CommunicationIdentifier), "Expected AddParticipants to contain user4");
                Assert.IsTrue(updateRoomParticipantsResult.Any(x => x.CommunicationIdentifier == participant5.CommunicationIdentifier), "Expected AddParticipants to contain user5");
                Assert.IsTrue(updateRoomParticipantsResult.Any(x => x.Role == participant4.Role), "Expected AddParticipants to contain role4");
                Assert.IsTrue(updateRoomParticipantsResult.Any(x => x.Role == participant5.Role), "Expected AddParticipants to contain role5");

                // Get Room Participants
                Response<ParticipantsCollection> getParticipantsResponse = await roomsClient.GetParticipantsAsync(createdRoomId);
                Assert.AreEqual(getParticipantsResponse.Value.Participants.ToList().Count, updateParticipantsRoom.Id);

                // Delete room
                Response deleteRoomResponse = await roomsClient.DeleteRoomAsync(createdRoomId);
                Assert.AreEqual(204, deleteRoomResponse.Status);
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail($"Unexpected error: {ex}");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected error: {ex}");
            }
        }

        [Test]
        public async Task AcsRoomTimePartialUpdateLiveTest()
        {
            RoomsClient roomsClient = CreateInstrumentedRoomsClient(RoomsClientOptions.ServiceVersion.V2022_02_01_Preview);

            var validFrom = DateTime.Now;
            var validUntil = validFrom.AddDays(1);

            try
            {
                Response<RoomModel> createRoomResponse = await roomsClient.CreateRoomAsync(default, validFrom);
                RoomModel createCommunicationRoom = createRoomResponse.Value;
                Assert.IsFalse(string.IsNullOrWhiteSpace(createCommunicationRoom.Id));
                Assert.AreEqual(validFrom, createCommunicationRoom.ValidFrom);

                var createdRoomId = createCommunicationRoom.Id;

                Response<RoomModel> updateRoomResponse = await roomsClient.UpdateRoomAsync(createdRoomId, validFrom, validUntil);
                RoomModel updateCommunicationRoom = updateRoomResponse.Value;
                Assert.AreEqual(createdRoomId, updateCommunicationRoom.Id);
                Assert.AreEqual(validFrom, updateCommunicationRoom.ValidFrom);
                Assert.AreEqual(validUntil, updateCommunicationRoom.ValidUntil);

                Response deleteRoomResponse = await roomsClient.DeleteRoomAsync(createdRoomId);
                Assert.AreEqual(204, deleteRoomResponse.Status);
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail($"Unexpected error: {ex}");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected error: {ex}");
            }
        }
    }
}
