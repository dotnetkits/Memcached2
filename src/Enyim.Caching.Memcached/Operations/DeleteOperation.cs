﻿using System;
using System.Buffers;

namespace Enyim.Caching.Memcached.Operations
{
	internal class DeleteOperation : BinaryItemOperation, ICanBeSilent
	{
		public DeleteOperation(MemoryPool<byte> allocator, string key, IKeyFormatter keyFormatter)
			: base(allocator, key, keyFormatter)
		{ }

		public bool Silent { get; set; }

		/*

			Request:

			MUST NOT have extras.
			MUST have key.
			MUST NOT have value.

		*/
		public void Initialize()
		{
			try
			{
				Request.Operation = Silent ? OpCode.DeleteQ : OpCode.Delete;
				Request.Cas = Cas;
				Request.Commit();
			}
			catch
			{
				Request?.Dispose();
				throw;
			}
		}

		/*

			Response:

			MUST NOT have extras.
			MUST NOT have key.
			MUST NOT have value.

		*/
		protected override bool ParseResult(BinaryResponse? response)
		{
			if (response == null)
			{
				StatusCode = Protocol.Status.Success;
			}
			else
			{
				response.MustBeEmpty();
			}

			return false;
		}
	}
}

#region [ License information          ]

/*

Copyright (c) Attila Kiskó, enyim.com

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

  http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

*/

#endregion
