﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Gdax
{
	public static class GetAccountHistoryQuery
	{
		public static async Task<PaginatedResult<Ledger>> GetAccountHistoryAsync(this GdaxClient client, Guid accountId, PaginationOptions paging = null)
		{
			Check.NotNull(client, nameof(client));
			Check.NotEmpty(accountId, nameof(accountId));

			var request = new GdaxRequestBuilder($"/accounts/{accountId}/ledger")
				.SetPageOptions(paging)
				.Build();

			var response = await client.GetResponseAsync<IList<Ledger>>(request).ConfigureAwait(false);

			return new PaginatedResult<Ledger>(response, paging);
		}

		public class Ledger
		{
			[JsonProperty("id")]
			public Int32 Id { get; set; }
			
			[JsonProperty("created_at")]
			public DateTime CreatedAt { get; set; }

			[JsonProperty("amount")]
			public Decimal Amount { get; set; }

			[JsonProperty("balance")]
			public Decimal Balance { get; set; }

			[JsonProperty("type")]
			public LedgerEntryType Type { get; set; }

			[JsonProperty("details")]
			public LedgerDetails Details { get; set; }
		}

		public enum LedgerEntryType
		{
			/// <summary>
			/// Funds moved to/from Coinbase to GDAX
			/// </summary>
			Transfer,

			/// <summary>
			/// Funds moved as a result of a trade
			/// </summary>
			Match,

			/// <summary>
			/// Fee as a result of a trade
			/// </summary>
			Fee,

			/// <summary>
			/// Fee rebate as per our fee schedule
			/// </summary>
			Rebate
		}

		public class LedgerDetails
		{
			[JsonProperty("order_id")]
			public Guid? OrderId { get; set; }

			[JsonProperty("trade_id")]
			public Int32? TradeId { get; set; }

			[JsonProperty("product_id")]
			public String ProductId { get; set; }

			[JsonProperty("transfer_id")]
			public Guid? TransferId { get; set; }

			[JsonProperty("transfer_type")]
			public String TransferType { get; set; }
		}
	}

}
