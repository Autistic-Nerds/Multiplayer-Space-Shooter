namespace CosmosEngine.Netcode
{
	[System.Serializable]
	public class RemoteProcedureCall
	{
		//When an RPC is invoked
		//Check if an identical (same ID and method) RPC message is within the queue.
		//If an identical RPC exist, replace it with the newest RPC.
		//Add the RPC to a call queue

		//Send the RPC to the reciever.
		//Every RTT resend the package.

		//When receiver gets the message.
		//Send an acknowledge message to sender, confirming that the RPC was recieved.
		//Remove the RPC from the call queue.

		public string Method { get; set; }
		public uint Index { get; set; }
		public string Args { get; set; }
	}
}