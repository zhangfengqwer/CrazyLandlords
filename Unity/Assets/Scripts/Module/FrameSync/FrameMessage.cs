﻿using System.Collections.Generic;
using ProtoBuf;

namespace ETModel
{
	[Message(Opcode.OneFrameMessage)]
	[ProtoContract]
	public class OneFrameMessage: IMessage
	{
		[ProtoMember(1, IsRequired = true)]
		public ushort Op;

		[ProtoMember(2, IsRequired = true)]
		public byte[] AMessage;
	}

	[Message(Opcode.FrameMessage)]
	[ProtoContract]
	public partial class FrameMessage : IActorMessage
	{
		[ProtoMember(1, IsRequired = true)]
		public int Frame;

		[ProtoMember(2)]
		public List<OneFrameMessage> Messages = new List<OneFrameMessage>();
	}
}
