/**
*            Module: TokenApi.cs
*       Description: Represents a collection of functions to interact with the API endpoints
*            Author: Moralis Web3 Technology AB, 559307-5988 - David B. Goodrich
*  
* NOTE: THIS FILE HAS BEEN AUTOMATICALLY GENERATED. ANY CHANGES MADE TO THIS 
* FILE WILL BE LOST
*
* MIT License
*  
* Copyright (c) 2022 Moralis Web3 Technology AB, 559307-5988
*  
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the 'Software'), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/ 
            using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using Moralis.Web3Api.Client;
using Moralis.Web3Api.Interfaces;
using Moralis.Web3Api.Models;

namespace Moralis.Web3Api.Api
{
	/// <summary>
	/// Represents a collection of functions to interact with the API endpoints
	/// </summary>
	public class TokenApi : ITokenApi
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TokenApi"/> class.
		/// </summary>
		/// <param name="apiClient"> an instance of ApiClient (optional)</param>
		/// <returns></returns>
		public TokenApi(ApiClient apiClient = null)
		{
			if (apiClient == null) // use the default one in Configuration
				this.ApiClient = Configuration.DefaultApiClient; 
			else
				this.ApiClient = apiClient;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TokenApi"/> class.
		/// </summary>
		/// <returns></returns>
		public TokenApi(String basePath)
		{
			this.ApiClient = new ApiClient(basePath);
		}

		/// <summary>
		/// Sets the base path of the API client.
		/// </summary>
		/// <param name="basePath">The base path</param>
		/// <value>The base path</value>
		public void SetBasePath(String basePath)
		{
			this.ApiClient.BasePath = basePath;
		}

		/// <summary>
		/// Gets the base path of the API client.
		/// </summary>
		/// <param name="basePath">The base path</param>
		/// <value>The base path</value>
		public String GetBasePath(String basePath)
		{
			return this.ApiClient.BasePath;
		}

		/// <summary>
		/// Gets or sets the API client.
		/// </summary>
		/// <value>An instance of the ApiClient</value>
		public ApiClient ApiClient {get; set;}


		/// <summary>
		/// Returns metadata (name, symbol, decimals, logo) for a given token contract address.
		/// </summary>
		/// <param name="addresses">The addresses to get metadata for</param>
		/// <param name="chain">The chain to query</param>
		/// <param name="subdomain">The subdomain of the moralis server to use (Only use when selecting local devchain as chain)</param>
		/// <param name="providerUrl">web3 provider url to user when using local dev chain</param>
		/// <returns>Returns metadata (name, symbol, decimals, logo) for a given token contract address.</returns>
		public async Task<List<Erc20Metadata>> GetTokenMetadata (List<String> addresses, ChainList chain, string subdomain=null, string providerUrl=null)
		{

			// Verify the required parameter 'addresses' is set
			if (addresses == null) throw new ApiException(400, "Missing required parameter 'addresses' when calling GetTokenMetadata");

			var postBody = new Dictionary<String, String>();
			var queryParams = new Dictionary<String, String>();
			var headerParams = new Dictionary<String, String>();
			var formParams = new Dictionary<String, String>();
			var fileParams = new Dictionary<String, FileParameter>();

			var path = "/erc20/metadata";
			path = path.Replace("{format}", "json");

			if(addresses != null) queryParams.Add("addresses", ApiClient.ParameterToString(addresses));
			queryParams.Add("chain", ApiClient.ParameterToHex((long)chain));
			if(subdomain != null) queryParams.Add("subdomain", ApiClient.ParameterToString(subdomain));
			if(providerUrl != null) queryParams.Add("providerUrl", ApiClient.ParameterToString(providerUrl));

			// Authentication setting, if any
			String[] authSettings = new String[] { "ApiKeyAuth" };

			string bodyData = postBody.Count > 0 ? JsonConvert.SerializeObject(postBody) : null;

			IRestResponse response = (IRestResponse)(await ApiClient.CallApi(path, Method.GET, queryParams, bodyData, headerParams, formParams, fileParams, authSettings));

			if (((int)response.StatusCode) >= 400)
				throw new ApiException((int)response.StatusCode, "Error calling GetTokenMetadata: " + response.Content, response.Content);
			else if (((int)response.StatusCode) == 0)
				throw new ApiException((int)response.StatusCode, "Error calling GetTokenMetadata: " + response.ErrorMessage, response.ErrorMessage);

			return (List<Erc20Metadata>)ApiClient.Deserialize(response.Content, typeof(List<Erc20Metadata>), response.Headers);
		}
		/// <summary>
		/// Get the nft trades for a given contracts and marketplace
		/// </summary>
		/// <param name="address">Address of the contract</param>
		/// <param name="chain">The chain to query</param>
		/// <param name="fromBlock">The minimum block number from where to get the transfers
		/// * Provide the param 'from_block' or 'from_date'
		/// * If 'from_date' and 'from_block' are provided, 'from_block' will be used.
		/// </param>
		/// <param name="toBlock">To get the reserves at this block number</param>
		/// <param name="fromDate">The date from where to get the transfers (any format that is accepted by momentjs)
		/// * Provide the param 'from_block' or 'from_date'
		/// * If 'from_date' and 'from_block' are provided, 'from_block' will be used.
		/// </param>
		/// <param name="toDate">Get the reserves to this date (any format that is accepted by momentjs)
		/// * Provide the param 'to_block' or 'to_date'
		/// * If 'to_date' and 'to_block' are provided, 'to_block' will be used.
		/// </param>
		/// <param name="providerUrl">web3 provider url to user when using local dev chain</param>
		/// <param name="marketplace">marketplace from where to get the trades (only opensea is supported at the moment)</param>
		/// <param name="offset">offset</param>
		/// <param name="limit">limit</param>
		/// <returns>Returns the trades</returns>
		public async Task<TradeCollection> GetNFTTrades (string address, ChainList chain, int? fromBlock=null, string toBlock=null, string fromDate=null, string toDate=null, string providerUrl=null, string marketplace=null, int? offset=null, int? limit=null)
		{

			// Verify the required parameter 'address' is set
			if (address == null) throw new ApiException(400, "Missing required parameter 'address' when calling GetNFTTrades");

			var postBody = new Dictionary<String, String>();
			var queryParams = new Dictionary<String, String>();
			var headerParams = new Dictionary<String, String>();
			var formParams = new Dictionary<String, String>();
			var fileParams = new Dictionary<String, FileParameter>();

			var path = "/nft/{address}/trades";
			path = path.Replace("{format}", "json");
			path = path.Replace("{" + "address" + "}", ApiClient.ParameterToString(address));
			queryParams.Add("chain", ApiClient.ParameterToHex((long)chain));
			if(fromBlock != null) queryParams.Add("from_block", ApiClient.ParameterToString(fromBlock));
			if(toBlock != null) queryParams.Add("to_block", ApiClient.ParameterToString(toBlock));
			if(fromDate != null) queryParams.Add("from_date", ApiClient.ParameterToString(fromDate));
			if(toDate != null) queryParams.Add("to_date", ApiClient.ParameterToString(toDate));
			if(providerUrl != null) queryParams.Add("provider_url", ApiClient.ParameterToString(providerUrl));
			if(marketplace != null) queryParams.Add("marketplace", ApiClient.ParameterToString(marketplace));
			if(offset != null) queryParams.Add("offset", ApiClient.ParameterToString(offset));
			if(limit != null) queryParams.Add("limit", ApiClient.ParameterToString(limit));

			// Authentication setting, if any
			String[] authSettings = new String[] { "ApiKeyAuth" };

			string bodyData = postBody.Count > 0 ? JsonConvert.SerializeObject(postBody) : null;

			IRestResponse response = (IRestResponse)(await ApiClient.CallApi(path, Method.GET, queryParams, bodyData, headerParams, formParams, fileParams, authSettings));

			if (((int)response.StatusCode) >= 400)
				throw new ApiException((int)response.StatusCode, "Error calling GetNFTTrades: " + response.Content, response.Content);
			else if (((int)response.StatusCode) == 0)
				throw new ApiException((int)response.StatusCode, "Error calling GetNFTTrades: " + response.ErrorMessage, response.ErrorMessage);

			return (TradeCollection)ApiClient.Deserialize(response.Content, typeof(TradeCollection), response.Headers);
		}
		/// <summary>
		/// Get the lowest price found for a nft token contract for the last x days (only trades paid in ETH)
		/// </summary>
		/// <param name="address">Address of the contract</param>
		/// <param name="chain">The chain to query</param>
		/// <param name="days">The number of days to look back to find the lowest price
		/// If not provided 7 days will be the default
		/// </param>
		/// <param name="providerUrl">web3 provider url to user when using local dev chain</param>
		/// <param name="marketplace">marketplace from where to get the trades (only opensea is supported at the moment)</param>
		/// <returns>Returns the trade with the lowest price</returns>
		public async Task<Trade> GetNFTLowestPrice (string address, ChainList chain, int? days=null, string providerUrl=null, string marketplace=null)
		{

			// Verify the required parameter 'address' is set
			if (address == null) throw new ApiException(400, "Missing required parameter 'address' when calling GetNFTLowestPrice");

			var postBody = new Dictionary<String, String>();
			var queryParams = new Dictionary<String, String>();
			var headerParams = new Dictionary<String, String>();
			var formParams = new Dictionary<String, String>();
			var fileParams = new Dictionary<String, FileParameter>();

			var path = "/nft/{address}/lowestprice";
			path = path.Replace("{format}", "json");
			path = path.Replace("{" + "address" + "}", ApiClient.ParameterToString(address));
			queryParams.Add("chain", ApiClient.ParameterToHex((long)chain));
			if(days != null) queryParams.Add("days", ApiClient.ParameterToString(days));
			if(providerUrl != null) queryParams.Add("provider_url", ApiClient.ParameterToString(providerUrl));
			if(marketplace != null) queryParams.Add("marketplace", ApiClient.ParameterToString(marketplace));

			// Authentication setting, if any
			String[] authSettings = new String[] { "ApiKeyAuth" };

			string bodyData = postBody.Count > 0 ? JsonConvert.SerializeObject(postBody) : null;

			IRestResponse response = (IRestResponse)(await ApiClient.CallApi(path, Method.GET, queryParams, bodyData, headerParams, formParams, fileParams, authSettings));

			if (((int)response.StatusCode) >= 400)
				throw new ApiException((int)response.StatusCode, "Error calling GetNFTLowestPrice: " + response.Content, response.Content);
			else if (((int)response.StatusCode) == 0)
				throw new ApiException((int)response.StatusCode, "Error calling GetNFTLowestPrice: " + response.ErrorMessage, response.ErrorMessage);

			return (Trade)ApiClient.Deserialize(response.Content, typeof(Trade), response.Headers);
		}
		/// <summary>
		/// Returns metadata (name, symbol, decimals, logo) for a given token contract address.
		/// </summary>
		/// <param name="symbols">The symbols to get metadata for</param>
		/// <param name="chain">The chain to query</param>
		/// <param name="subdomain">The subdomain of the moralis server to use (Only use when selecting local devchain as chain)</param>
		/// <returns>Returns metadata (name, symbol, decimals, logo) for a given token contract address.</returns>
		public async Task<List<Erc20Metadata>> GetTokenMetadataBySymbol (List<String> symbols, ChainList chain, string subdomain=null)
		{

			// Verify the required parameter 'symbols' is set
			if (symbols == null) throw new ApiException(400, "Missing required parameter 'symbols' when calling GetTokenMetadataBySymbol");

			var postBody = new Dictionary<String, String>();
			var queryParams = new Dictionary<String, String>();
			var headerParams = new Dictionary<String, String>();
			var formParams = new Dictionary<String, String>();
			var fileParams = new Dictionary<String, FileParameter>();

			var path = "/erc20/metadata/symbols";
			path = path.Replace("{format}", "json");

			if(symbols != null) queryParams.Add("symbols", ApiClient.ParameterToString(symbols));
			queryParams.Add("chain", ApiClient.ParameterToHex((long)chain));
			if(subdomain != null) queryParams.Add("subdomain", ApiClient.ParameterToString(subdomain));

			// Authentication setting, if any
			String[] authSettings = new String[] { "ApiKeyAuth" };

			string bodyData = postBody.Count > 0 ? JsonConvert.SerializeObject(postBody) : null;

			IRestResponse response = (IRestResponse)(await ApiClient.CallApi(path, Method.GET, queryParams, bodyData, headerParams, formParams, fileParams, authSettings));

			if (((int)response.StatusCode) >= 400)
				throw new ApiException((int)response.StatusCode, "Error calling GetTokenMetadataBySymbol: " + response.Content, response.Content);
			else if (((int)response.StatusCode) == 0)
				throw new ApiException((int)response.StatusCode, "Error calling GetTokenMetadataBySymbol: " + response.ErrorMessage, response.ErrorMessage);

			return (List<Erc20Metadata>)ApiClient.Deserialize(response.Content, typeof(List<Erc20Metadata>), response.Headers);
		}
		/// <summary>
		/// Returns the price nominated in the native token and usd for a given token contract address.
		/// </summary>
		/// <param name="address">The address of the token contract</param>
		/// <param name="chain">The chain to query</param>
		/// <param name="providerUrl">web3 provider url to user when using local dev chain</param>
		/// <param name="exchange">The factory name or address of the token exchange</param>
		/// <param name="toBlock">to_block</param>
		/// <returns>Returns the price nominated in the native token and usd for a given token contract address</returns>
		public async Task<Erc20Price> GetTokenPrice (string address, ChainList chain, string providerUrl=null, string exchange=null, int? toBlock=null)
		{

			// Verify the required parameter 'address' is set
			if (address == null) throw new ApiException(400, "Missing required parameter 'address' when calling GetTokenPrice");

			var postBody = new Dictionary<String, String>();
			var queryParams = new Dictionary<String, String>();
			var headerParams = new Dictionary<String, String>();
			var formParams = new Dictionary<String, String>();
			var fileParams = new Dictionary<String, FileParameter>();

			var path = "/erc20/{address}/price";
			path = path.Replace("{format}", "json");
			path = path.Replace("{" + "address" + "}", ApiClient.ParameterToString(address));
			queryParams.Add("chain", ApiClient.ParameterToHex((long)chain));
			if(providerUrl != null) queryParams.Add("providerUrl", ApiClient.ParameterToString(providerUrl));
			if(exchange != null) queryParams.Add("exchange", ApiClient.ParameterToString(exchange));
			if(toBlock != null) queryParams.Add("to_block", ApiClient.ParameterToString(toBlock));

			// Authentication setting, if any
			String[] authSettings = new String[] { "ApiKeyAuth" };

			string bodyData = postBody.Count > 0 ? JsonConvert.SerializeObject(postBody) : null;

			IRestResponse response = (IRestResponse)(await ApiClient.CallApi(path, Method.GET, queryParams, bodyData, headerParams, formParams, fileParams, authSettings));

			if (((int)response.StatusCode) >= 400)
				throw new ApiException((int)response.StatusCode, "Error calling GetTokenPrice: " + response.Content, response.Content);
			else if (((int)response.StatusCode) == 0)
				throw new ApiException((int)response.StatusCode, "Error calling GetTokenPrice: " + response.ErrorMessage, response.ErrorMessage);

			return (Erc20Price)ApiClient.Deserialize(response.Content, typeof(Erc20Price), response.Headers);
		}
		/// <summary>
		/// Gets ERC20 token contract transactions in descending order based on block number
		/// </summary>
		/// <param name="address">The address of the token contract</param>
		/// <param name="chain">The chain to query</param>
		/// <param name="subdomain">The subdomain of the moralis server to use (Only use when selecting local devchain as chain)</param>
		/// <param name="fromBlock">The minimum block number from where to get the transfers
		/// * Provide the param 'from_block' or 'from_date'
		/// * If 'from_date' and 'from_block' are provided, 'from_block' will be used.
		/// </param>
		/// <param name="toBlock">The maximum block number from where to get the transfers.
		/// * Provide the param 'to_block' or 'to_date'
		/// * If 'to_date' and 'to_block' are provided, 'to_block' will be used.
		/// </param>
		/// <param name="fromDate">The date from where to get the transfers (any format that is accepted by momentjs)
		/// * Provide the param 'from_block' or 'from_date'
		/// * If 'from_date' and 'from_block' are provided, 'from_block' will be used.
		/// </param>
		/// <param name="toDate">Get the transfers to this date (any format that is accepted by momentjs)
		/// * Provide the param 'to_block' or 'to_date'
		/// * If 'to_date' and 'to_block' are provided, 'to_block' will be used.
		/// </param>
		/// <param name="offset">offset</param>
		/// <param name="limit">limit</param>
		/// <returns>Returns a collection of token contract transactions.</returns>
		public async Task<Erc20TransactionCollection> GetTokenAddressTransfers (string address, ChainList chain, string subdomain=null, int? fromBlock=null, int? toBlock=null, string fromDate=null, string toDate=null, int? offset=null, int? limit=null)
		{

			// Verify the required parameter 'address' is set
			if (address == null) throw new ApiException(400, "Missing required parameter 'address' when calling GetTokenAddressTransfers");

			var postBody = new Dictionary<String, String>();
			var queryParams = new Dictionary<String, String>();
			var headerParams = new Dictionary<String, String>();
			var formParams = new Dictionary<String, String>();
			var fileParams = new Dictionary<String, FileParameter>();

			var path = "/erc20/{address}/transfers";
			path = path.Replace("{format}", "json");
			path = path.Replace("{" + "address" + "}", ApiClient.ParameterToString(address));
			queryParams.Add("chain", ApiClient.ParameterToHex((long)chain));
			if(subdomain != null) queryParams.Add("subdomain", ApiClient.ParameterToString(subdomain));
			if(fromBlock != null) queryParams.Add("from_block", ApiClient.ParameterToString(fromBlock));
			if(toBlock != null) queryParams.Add("to_block", ApiClient.ParameterToString(toBlock));
			if(fromDate != null) queryParams.Add("from_date", ApiClient.ParameterToString(fromDate));
			if(toDate != null) queryParams.Add("to_date", ApiClient.ParameterToString(toDate));
			if(offset != null) queryParams.Add("offset", ApiClient.ParameterToString(offset));
			if(limit != null) queryParams.Add("limit", ApiClient.ParameterToString(limit));

			// Authentication setting, if any
			String[] authSettings = new String[] { "ApiKeyAuth" };

			string bodyData = postBody.Count > 0 ? JsonConvert.SerializeObject(postBody) : null;

			IRestResponse response = (IRestResponse)(await ApiClient.CallApi(path, Method.GET, queryParams, bodyData, headerParams, formParams, fileParams, authSettings));

			if (((int)response.StatusCode) >= 400)
				throw new ApiException((int)response.StatusCode, "Error calling GetTokenAddressTransfers: " + response.Content, response.Content);
			else if (((int)response.StatusCode) == 0)
				throw new ApiException((int)response.StatusCode, "Error calling GetTokenAddressTransfers: " + response.ErrorMessage, response.ErrorMessage);

			return (Erc20TransactionCollection)ApiClient.Deserialize(response.Content, typeof(Erc20TransactionCollection), response.Headers);
		}
		/// <summary>
		/// Gets the amount which the spender is allowed to withdraw from the spender
		/// </summary>
		/// <param name="address">The address of the token contract</param>
		/// <param name="ownerAddress">The address of the token owner</param>
		/// <param name="spenderAddress">The address of the token spender</param>
		/// <param name="chain">The chain to query</param>
		/// <param name="providerUrl">web3 provider url to user when using local dev chain</param>
		/// <returns>Returns the amount which the spender is allowed to withdraw from the owner..</returns>
		public async Task<Erc20Allowance> GetTokenAllowance (string address, string ownerAddress, string spenderAddress, ChainList chain, string providerUrl=null)
		{

			// Verify the required parameter 'address' is set
			if (address == null) throw new ApiException(400, "Missing required parameter 'address' when calling GetTokenAllowance");

			// Verify the required parameter 'ownerAddress' is set
			if (ownerAddress == null) throw new ApiException(400, "Missing required parameter 'ownerAddress' when calling GetTokenAllowance");

			// Verify the required parameter 'spenderAddress' is set
			if (spenderAddress == null) throw new ApiException(400, "Missing required parameter 'spenderAddress' when calling GetTokenAllowance");

			var postBody = new Dictionary<String, String>();
			var queryParams = new Dictionary<String, String>();
			var headerParams = new Dictionary<String, String>();
			var formParams = new Dictionary<String, String>();
			var fileParams = new Dictionary<String, FileParameter>();

			var path = "/erc20/{address}/allowance";
			path = path.Replace("{format}", "json");
			path = path.Replace("{" + "address" + "}", ApiClient.ParameterToString(address));
			if(ownerAddress != null) queryParams.Add("owner_address", ApiClient.ParameterToString(ownerAddress));
			if(spenderAddress != null) queryParams.Add("spender_address", ApiClient.ParameterToString(spenderAddress));
			queryParams.Add("chain", ApiClient.ParameterToHex((long)chain));
			if(providerUrl != null) queryParams.Add("providerUrl", ApiClient.ParameterToString(providerUrl));

			// Authentication setting, if any
			String[] authSettings = new String[] { "ApiKeyAuth" };

			string bodyData = postBody.Count > 0 ? JsonConvert.SerializeObject(postBody) : null;

			IRestResponse response = (IRestResponse)(await ApiClient.CallApi(path, Method.GET, queryParams, bodyData, headerParams, formParams, fileParams, authSettings));

			if (((int)response.StatusCode) >= 400)
				throw new ApiException((int)response.StatusCode, "Error calling GetTokenAllowance: " + response.Content, response.Content);
			else if (((int)response.StatusCode) == 0)
				throw new ApiException((int)response.StatusCode, "Error calling GetTokenAllowance: " + response.ErrorMessage, response.ErrorMessage);

			return (Erc20Allowance)ApiClient.Deserialize(response.Content, typeof(Erc20Allowance), response.Headers);
		}
		/// <summary>
		/// Gets NFTs that match a given metadata search.
		/// </summary>
		/// <param name="q">The search string</param>
		/// <param name="chain">The chain to query</param>
		/// <param name="format">The format of the token id</param>
		/// <param name="filter">What fields the search should match on. To look into the entire metadata set the value to 'global'. To have a better response time you can look into a specific field like name</param>
		/// <param name="fromBlock">The minimum block number from where to start the search
		/// * Provide the param 'from_block' or 'from_date'
		/// * If 'from_date' and 'from_block' are provided, 'from_block' will be used.
		/// </param>
		/// <param name="toBlock">The maximum block number from where to end the search
		/// * Provide the param 'to_block' or 'to_date'
		/// * If 'to_date' and 'to_block' are provided, 'to_block' will be used.
		/// </param>
		/// <param name="fromDate">The date from where to start the search (any format that is accepted by momentjs)
		/// * Provide the param 'from_block' or 'from_date'
		/// * If 'from_date' and 'from_block' are provided, 'from_block' will be used.
		/// </param>
		/// <param name="toDate">Get search results up until this date (any format that is accepted by momentjs)
		/// * Provide the param 'to_block' or 'to_date'
		/// * If 'to_date' and 'to_block' are provided, 'to_block' will be used.
		/// </param>
		/// <param name="offset">offset</param>
		/// <param name="limit">limit</param>
		/// <returns>Returns the matching NFTs</returns>
		public async Task<NftMetadataCollection> SearchNFTs (string q, ChainList chain, string format=null, string filter=null, int? fromBlock=null, int? toBlock=null, string fromDate=null, string toDate=null, int? offset=null, int? limit=null)
		{

			// Verify the required parameter 'q' is set
			if (q == null) throw new ApiException(400, "Missing required parameter 'q' when calling SearchNFTs");

			var postBody = new Dictionary<String, String>();
			var queryParams = new Dictionary<String, String>();
			var headerParams = new Dictionary<String, String>();
			var formParams = new Dictionary<String, String>();
			var fileParams = new Dictionary<String, FileParameter>();

			var path = "/nft/search";
			path = path.Replace("{format}", "json");

			if(q != null) queryParams.Add("q", ApiClient.ParameterToString(q));
			queryParams.Add("chain", ApiClient.ParameterToHex((long)chain));
			if(format != null) queryParams.Add("format", ApiClient.ParameterToString(format));
			if(filter != null) queryParams.Add("filter", ApiClient.ParameterToString(filter));
			if(fromBlock != null) queryParams.Add("from_block", ApiClient.ParameterToString(fromBlock));
			if(toBlock != null) queryParams.Add("to_block", ApiClient.ParameterToString(toBlock));
			if(fromDate != null) queryParams.Add("from_date", ApiClient.ParameterToString(fromDate));
			if(toDate != null) queryParams.Add("to_date", ApiClient.ParameterToString(toDate));
			if(offset != null) queryParams.Add("offset", ApiClient.ParameterToString(offset));
			if(limit != null) queryParams.Add("limit", ApiClient.ParameterToString(limit));

			// Authentication setting, if any
			String[] authSettings = new String[] { "ApiKeyAuth" };

			string bodyData = postBody.Count > 0 ? JsonConvert.SerializeObject(postBody) : null;

			IRestResponse response = (IRestResponse)(await ApiClient.CallApi(path, Method.GET, queryParams, bodyData, headerParams, formParams, fileParams, authSettings));

			if (((int)response.StatusCode) >= 400)
				throw new ApiException((int)response.StatusCode, "Error calling SearchNFTs: " + response.Content, response.Content);
			else if (((int)response.StatusCode) == 0)
				throw new ApiException((int)response.StatusCode, "Error calling SearchNFTs: " + response.ErrorMessage, response.ErrorMessage);

			return (NftMetadataCollection)ApiClient.Deserialize(response.Content, typeof(NftMetadataCollection), response.Headers);
		}
		/// <summary>
		/// Gets the transfers of the tokens from a block number to a block number
		/// </summary>
		/// <param name="chain">The chain to query</param>
		/// <param name="fromBlock">The minimum block number from where to get the transfers
		/// * Provide the param 'from_block' or 'from_date'
		/// * If 'from_date' and 'from_block' are provided, 'from_block' will be used.
		/// </param>
		/// <param name="toBlock">The maximum block number from where to get the transfers.
		/// * Provide the param 'to_block' or 'to_date'
		/// * If 'to_date' and 'to_block' are provided, 'to_block' will be used.
		/// </param>
		/// <param name="fromDate">The date from where to get the transfers (any format that is accepted by momentjs)
		/// * Provide the param 'from_block' or 'from_date'
		/// * If 'from_date' and 'from_block' are provided, 'from_block' will be used.
		/// </param>
		/// <param name="toDate">Get transfers up until this date (any format that is accepted by momentjs)
		/// * Provide the param 'to_block' or 'to_date'
		/// * If 'to_date' and 'to_block' are provided, 'to_block' will be used.
		/// </param>
		/// <param name="format">The format of the token id</param>
		/// <param name="offset">offset</param>
		/// <param name="limit">limit</param>
		/// <param name="cursor">The cursor returned in the last response (for getting the next page)
		/// </param>
		/// <returns>Returns a collection of NFT transfers</returns>
		public async Task<NftTransferCollection> GetNftTransfersFromToBlock (ChainList chain, int? fromBlock=null, int? toBlock=null, string fromDate=null, string toDate=null, string format=null, int? offset=null, int? limit=null, string cursor=null)
		{

			var postBody = new Dictionary<String, String>();
			var queryParams = new Dictionary<String, String>();
			var headerParams = new Dictionary<String, String>();
			var formParams = new Dictionary<String, String>();
			var fileParams = new Dictionary<String, FileParameter>();

			var path = "/nft/transfers";
			path = path.Replace("{format}", "json");

			queryParams.Add("chain", ApiClient.ParameterToHex((long)chain));
			if(fromBlock != null) queryParams.Add("from_block", ApiClient.ParameterToString(fromBlock));
			if(toBlock != null) queryParams.Add("to_block", ApiClient.ParameterToString(toBlock));
			if(fromDate != null) queryParams.Add("from_date", ApiClient.ParameterToString(fromDate));
			if(toDate != null) queryParams.Add("to_date", ApiClient.ParameterToString(toDate));
			if(format != null) queryParams.Add("format", ApiClient.ParameterToString(format));
			if(offset != null) queryParams.Add("offset", ApiClient.ParameterToString(offset));
			if(limit != null) queryParams.Add("limit", ApiClient.ParameterToString(limit));
			if(cursor != null) queryParams.Add("cursor", ApiClient.ParameterToString(cursor));

			// Authentication setting, if any
			String[] authSettings = new String[] { "ApiKeyAuth" };

			string bodyData = postBody.Count > 0 ? JsonConvert.SerializeObject(postBody) : null;

			IRestResponse response = (IRestResponse)(await ApiClient.CallApi(path, Method.GET, queryParams, bodyData, headerParams, formParams, fileParams, authSettings));

			if (((int)response.StatusCode) >= 400)
				throw new ApiException((int)response.StatusCode, "Error calling GetNftTransfersFromToBlock: " + response.Content, response.Content);
			else if (((int)response.StatusCode) == 0)
				throw new ApiException((int)response.StatusCode, "Error calling GetNftTransfersFromToBlock: " + response.ErrorMessage, response.ErrorMessage);

			return (NftTransferCollection)ApiClient.Deserialize(response.Content, typeof(NftTransferCollection), response.Headers);
		}
		/// <summary>
		/// Gets data, including metadata (where available), for all token ids for the given contract address.
		/// * Results are sorted by the block the token id was minted (descending) and limited to 100 per page by default
		/// * Requests for contract addresses not yet indexed will automatically start the indexing process for that NFT collection
		/// 
		/// </summary>
		/// <param name="address">Address of the contract</param>
		/// <param name="chain">The chain to query</param>
		/// <param name="format">The format of the token id</param>
		/// <param name="offset">offset</param>
		/// <param name="limit">limit</param>
		/// <returns>Returns a collection of nfts</returns>
		public async Task<NftCollection> GetAllTokenIds (string address, ChainList chain, string format=null, int? offset=null, int? limit=null)
		{

			// Verify the required parameter 'address' is set
			if (address == null) throw new ApiException(400, "Missing required parameter 'address' when calling GetAllTokenIds");

			var postBody = new Dictionary<String, String>();
			var queryParams = new Dictionary<String, String>();
			var headerParams = new Dictionary<String, String>();
			var formParams = new Dictionary<String, String>();
			var fileParams = new Dictionary<String, FileParameter>();

			var path = "/nft/{address}";
			path = path.Replace("{format}", "json");
			path = path.Replace("{" + "address" + "}", ApiClient.ParameterToString(address));
			queryParams.Add("chain", ApiClient.ParameterToHex((long)chain));
			if(format != null) queryParams.Add("format", ApiClient.ParameterToString(format));
			if(offset != null) queryParams.Add("offset", ApiClient.ParameterToString(offset));
			if(limit != null) queryParams.Add("limit", ApiClient.ParameterToString(limit));

			// Authentication setting, if any
			String[] authSettings = new String[] { "ApiKeyAuth" };

			string bodyData = postBody.Count > 0 ? JsonConvert.SerializeObject(postBody) : null;

			IRestResponse response = (IRestResponse)(await ApiClient.CallApi(path, Method.GET, queryParams, bodyData, headerParams, formParams, fileParams, authSettings));

			if (((int)response.StatusCode) >= 400)
				throw new ApiException((int)response.StatusCode, "Error calling GetAllTokenIds: " + response.Content, response.Content);
			else if (((int)response.StatusCode) == 0)
				throw new ApiException((int)response.StatusCode, "Error calling GetAllTokenIds: " + response.ErrorMessage, response.ErrorMessage);

			return (NftCollection)ApiClient.Deserialize(response.Content, typeof(NftCollection), response.Headers);
		}
		/// <summary>
		/// Gets the transfers of the tokens matching the given parameters
		/// </summary>
		/// <param name="address">Address of the contract</param>
		/// <param name="chain">The chain to query</param>
		/// <param name="format">The format of the token id</param>
		/// <param name="offset">offset</param>
		/// <param name="limit">limit</param>
		/// <param name="cursor">The cursor returned in the last response (for getting the next page)
		/// </param>
		/// <returns>Returns a collection of NFT transfers</returns>
		public async Task<NftTransferCollection> GetContractNFTTransfers (string address, ChainList chain, string format=null, int? offset=null, int? limit=null, string cursor=null)
		{

			// Verify the required parameter 'address' is set
			if (address == null) throw new ApiException(400, "Missing required parameter 'address' when calling GetContractNFTTransfers");

			var postBody = new Dictionary<String, String>();
			var queryParams = new Dictionary<String, String>();
			var headerParams = new Dictionary<String, String>();
			var formParams = new Dictionary<String, String>();
			var fileParams = new Dictionary<String, FileParameter>();

			var path = "/nft/{address}/transfers";
			path = path.Replace("{format}", "json");
			path = path.Replace("{" + "address" + "}", ApiClient.ParameterToString(address));
			queryParams.Add("chain", ApiClient.ParameterToHex((long)chain));
			if(format != null) queryParams.Add("format", ApiClient.ParameterToString(format));
			if(offset != null) queryParams.Add("offset", ApiClient.ParameterToString(offset));
			if(limit != null) queryParams.Add("limit", ApiClient.ParameterToString(limit));
			if(cursor != null) queryParams.Add("cursor", ApiClient.ParameterToString(cursor));

			// Authentication setting, if any
			String[] authSettings = new String[] { "ApiKeyAuth" };

			string bodyData = postBody.Count > 0 ? JsonConvert.SerializeObject(postBody) : null;

			IRestResponse response = (IRestResponse)(await ApiClient.CallApi(path, Method.GET, queryParams, bodyData, headerParams, formParams, fileParams, authSettings));

			if (((int)response.StatusCode) >= 400)
				throw new ApiException((int)response.StatusCode, "Error calling GetContractNFTTransfers: " + response.Content, response.Content);
			else if (((int)response.StatusCode) == 0)
				throw new ApiException((int)response.StatusCode, "Error calling GetContractNFTTransfers: " + response.ErrorMessage, response.ErrorMessage);

			return (NftTransferCollection)ApiClient.Deserialize(response.Content, typeof(NftTransferCollection), response.Headers);
		}
		/// <summary>
		/// Gets all owners of NFT items within a given contract collection
		/// * Use after /nft/contract/{token_address} to find out who owns each token id in a collection
		/// * Make sure to include a sort parm on a column like block_number_minted for consistent pagination results
		/// * Requests for contract addresses not yet indexed will automatically start the indexing process for that NFT collection
		/// 
		/// </summary>
		/// <param name="address">Address of the contract</param>
		/// <param name="chain">The chain to query</param>
		/// <param name="format">The format of the token id</param>
		/// <param name="offset">offset</param>
		/// <param name="limit">limit</param>
		/// <returns>Returns a collection of nft owners</returns>
		public async Task<NftOwnerCollection> GetNFTOwners (string address, ChainList chain, string format=null, int? offset=null, int? limit=null)
		{

			// Verify the required parameter 'address' is set
			if (address == null) throw new ApiException(400, "Missing required parameter 'address' when calling GetNFTOwners");

			var postBody = new Dictionary<String, String>();
			var queryParams = new Dictionary<String, String>();
			var headerParams = new Dictionary<String, String>();
			var formParams = new Dictionary<String, String>();
			var fileParams = new Dictionary<String, FileParameter>();

			var path = "/nft/{address}/owners";
			path = path.Replace("{format}", "json");
			path = path.Replace("{" + "address" + "}", ApiClient.ParameterToString(address));
			queryParams.Add("chain", ApiClient.ParameterToHex((long)chain));
			if(format != null) queryParams.Add("format", ApiClient.ParameterToString(format));
			if(offset != null) queryParams.Add("offset", ApiClient.ParameterToString(offset));
			if(limit != null) queryParams.Add("limit", ApiClient.ParameterToString(limit));

			// Authentication setting, if any
			String[] authSettings = new String[] { "ApiKeyAuth" };

			string bodyData = postBody.Count > 0 ? JsonConvert.SerializeObject(postBody) : null;

			IRestResponse response = (IRestResponse)(await ApiClient.CallApi(path, Method.GET, queryParams, bodyData, headerParams, formParams, fileParams, authSettings));

			if (((int)response.StatusCode) >= 400)
				throw new ApiException((int)response.StatusCode, "Error calling GetNFTOwners: " + response.Content, response.Content);
			else if (((int)response.StatusCode) == 0)
				throw new ApiException((int)response.StatusCode, "Error calling GetNFTOwners: " + response.ErrorMessage, response.ErrorMessage);

			return (NftOwnerCollection)ApiClient.Deserialize(response.Content, typeof(NftOwnerCollection), response.Headers);
		}
		/// <summary>
		/// Gets the contract level metadata (name, symbol, base token uri) for the given contract
		/// * Requests for contract addresses not yet indexed will automatically start the indexing process for that NFT collection
		/// 
		/// </summary>
		/// <param name="address">Address of the contract</param>
		/// <param name="chain">The chain to query</param>
		/// <returns>Returns a collection NFT collections.</returns>
		public async Task<NftContractMetadata> GetNFTMetadata (string address, ChainList chain)
		{

			// Verify the required parameter 'address' is set
			if (address == null) throw new ApiException(400, "Missing required parameter 'address' when calling GetNFTMetadata");

			var postBody = new Dictionary<String, String>();
			var queryParams = new Dictionary<String, String>();
			var headerParams = new Dictionary<String, String>();
			var formParams = new Dictionary<String, String>();
			var fileParams = new Dictionary<String, FileParameter>();

			var path = "/nft/{address}/metadata";
			path = path.Replace("{format}", "json");
			path = path.Replace("{" + "address" + "}", ApiClient.ParameterToString(address));
			queryParams.Add("chain", ApiClient.ParameterToHex((long)chain));

			// Authentication setting, if any
			String[] authSettings = new String[] { "ApiKeyAuth" };

			string bodyData = postBody.Count > 0 ? JsonConvert.SerializeObject(postBody) : null;

			IRestResponse response = (IRestResponse)(await ApiClient.CallApi(path, Method.GET, queryParams, bodyData, headerParams, formParams, fileParams, authSettings));

			if (((int)response.StatusCode) >= 400)
				throw new ApiException((int)response.StatusCode, "Error calling GetNFTMetadata: " + response.Content, response.Content);
			else if (((int)response.StatusCode) == 0)
				throw new ApiException((int)response.StatusCode, "Error calling GetNFTMetadata: " + response.ErrorMessage, response.ErrorMessage);

			return (NftContractMetadata)ApiClient.Deserialize(response.Content, typeof(NftContractMetadata), response.Headers);
		}
		/// <summary>
		/// ReSync the metadata for an NFT
		/// * The metadata flag will request a resync of the metadata for an nft
		/// * The uri flag will request fetch the token_uri for an NFT and then fetch it's metadata. To be used when the token uri get updated
		/// 
		/// </summary>
		/// <param name="address">Address of the contract</param>
		/// <param name="tokenId">The id of the token</param>
		/// <param name="chain">The chain to query</param>
		/// <param name="flag">the type of resync to operate</param>
		public async Task<Type> ReSyncMetadata (string address, string tokenId, ChainList chain, string flag=null)
		{

			// Verify the required parameter 'address' is set
			if (address == null) throw new ApiException(400, "Missing required parameter 'address' when calling ReSyncMetadata");

			// Verify the required parameter 'tokenId' is set
			if (tokenId == null) throw new ApiException(400, "Missing required parameter 'tokenId' when calling ReSyncMetadata");

			var postBody = new Dictionary<String, String>();
			var queryParams = new Dictionary<String, String>();
			var headerParams = new Dictionary<String, String>();
			var formParams = new Dictionary<String, String>();
			var fileParams = new Dictionary<String, FileParameter>();

			var path = "/nft/{address}/{token_id}/metadata/resync";
			path = path.Replace("{format}", "json");
			path = path.Replace("{" + "address" + "}", ApiClient.ParameterToString(address));			path = path.Replace("{" + "token_id" + "}", ApiClient.ParameterToString(tokenId));
			queryParams.Add("chain", ApiClient.ParameterToHex((long)chain));
			if(flag != null) queryParams.Add("flag", ApiClient.ParameterToString(flag));

			// Authentication setting, if any
			String[] authSettings = new String[] { "ApiKeyAuth" };

			string bodyData = postBody.Count > 0 ? JsonConvert.SerializeObject(postBody) : null;

			IRestResponse response = (IRestResponse)(await ApiClient.CallApi(path, Method.GET, queryParams, bodyData, headerParams, formParams, fileParams, authSettings));

			if (((int)response.StatusCode) >= 400)
				throw new ApiException((int)response.StatusCode, "Error calling ReSyncMetadata: " + response.Content, response.Content);
			else if (((int)response.StatusCode) == 0)
				throw new ApiException((int)response.StatusCode, "Error calling ReSyncMetadata: " + response.ErrorMessage, response.ErrorMessage);

			return (Type)ApiClient.Deserialize(response.Content, typeof(Type), response.Headers);
		}
		/// <summary>
		/// Sync a Contract for NFT Index
		/// 
		/// </summary>
		/// <param name="address">Address of the contract</param>
		/// <param name="chain">The chain to query</param>
		public async Task<Type> SyncNFTContract (string address, ChainList chain)
		{

			// Verify the required parameter 'address' is set
			if (address == null) throw new ApiException(400, "Missing required parameter 'address' when calling SyncNFTContract");

			var postBody = new Dictionary<String, String>();
			var queryParams = new Dictionary<String, String>();
			var headerParams = new Dictionary<String, String>();
			var formParams = new Dictionary<String, String>();
			var fileParams = new Dictionary<String, FileParameter>();

			var path = "/nft/{address}/sync";
			path = path.Replace("{format}", "json");
			path = path.Replace("{" + "address" + "}", ApiClient.ParameterToString(address));
			queryParams.Add("chain", ApiClient.ParameterToHex((long)chain));

			// Authentication setting, if any
			String[] authSettings = new String[] { "ApiKeyAuth" };

			string bodyData = postBody.Count > 0 ? JsonConvert.SerializeObject(postBody) : null;

			IRestResponse response = (IRestResponse)(await ApiClient.CallApi(path, Method.PUT, queryParams, bodyData, headerParams, formParams, fileParams, authSettings));

			if (((int)response.StatusCode) >= 400)
				throw new ApiException((int)response.StatusCode, "Error calling SyncNFTContract: " + response.Content, response.Content);
			else if (((int)response.StatusCode) == 0)
				throw new ApiException((int)response.StatusCode, "Error calling SyncNFTContract: " + response.ErrorMessage, response.ErrorMessage);

			return (Type)ApiClient.Deserialize(response.Content, typeof(Type), response.Headers);
		}
		/// <summary>
		/// Gets data, including metadata (where available), for the given token id of the given contract address.
		/// * Requests for contract addresses not yet indexed will automatically start the indexing process for that NFT collection
		/// 
		/// </summary>
		/// <param name="address">Address of the contract</param>
		/// <param name="tokenId">The id of the token</param>
		/// <param name="chain">The chain to query</param>
		/// <param name="format">The format of the token id</param>
		/// <returns>Returns the specified NFT</returns>
		public async Task<Nft> GetTokenIdMetadata (string address, string tokenId, ChainList chain, string format=null)
		{

			// Verify the required parameter 'address' is set
			if (address == null) throw new ApiException(400, "Missing required parameter 'address' when calling GetTokenIdMetadata");

			// Verify the required parameter 'tokenId' is set
			if (tokenId == null) throw new ApiException(400, "Missing required parameter 'tokenId' when calling GetTokenIdMetadata");

			var postBody = new Dictionary<String, String>();
			var queryParams = new Dictionary<String, String>();
			var headerParams = new Dictionary<String, String>();
			var formParams = new Dictionary<String, String>();
			var fileParams = new Dictionary<String, FileParameter>();

			var path = "/nft/{address}/{token_id}";
			path = path.Replace("{format}", "json");
			path = path.Replace("{" + "address" + "}", ApiClient.ParameterToString(address));			path = path.Replace("{" + "token_id" + "}", ApiClient.ParameterToString(tokenId));
			queryParams.Add("chain", ApiClient.ParameterToHex((long)chain));
			if(format != null) queryParams.Add("format", ApiClient.ParameterToString(format));

			// Authentication setting, if any
			String[] authSettings = new String[] { "ApiKeyAuth" };

			string bodyData = postBody.Count > 0 ? JsonConvert.SerializeObject(postBody) : null;

			IRestResponse response = (IRestResponse)(await ApiClient.CallApi(path, Method.GET, queryParams, bodyData, headerParams, formParams, fileParams, authSettings));

			if (((int)response.StatusCode) >= 400)
				throw new ApiException((int)response.StatusCode, "Error calling GetTokenIdMetadata: " + response.Content, response.Content);
			else if (((int)response.StatusCode) == 0)
				throw new ApiException((int)response.StatusCode, "Error calling GetTokenIdMetadata: " + response.ErrorMessage, response.ErrorMessage);

			return (Nft)ApiClient.Deserialize(response.Content, typeof(Nft), response.Headers);
		}
		/// <summary>
		/// Gets all owners of NFT items within a given contract collection
		/// * Use after /nft/contract/{token_address} to find out who owns each token id in a collection
		/// * Make sure to include a sort parm on a column like block_number_minted for consistent pagination results
		/// * Requests for contract addresses not yet indexed will automatically start the indexing process for that NFT collection
		/// 
		/// </summary>
		/// <param name="address">Address of the contract</param>
		/// <param name="tokenId">The id of the token</param>
		/// <param name="chain">The chain to query</param>
		/// <param name="format">The format of the token id</param>
		/// <param name="offset">offset</param>
		/// <param name="limit">limit</param>
		/// <returns>Returns a collection of NFTs with their respective owners</returns>
		public async Task<NftOwnerCollection> GetTokenIdOwners (string address, string tokenId, ChainList chain, string format=null, int? offset=null, int? limit=null)
		{

			// Verify the required parameter 'address' is set
			if (address == null) throw new ApiException(400, "Missing required parameter 'address' when calling GetTokenIdOwners");

			// Verify the required parameter 'tokenId' is set
			if (tokenId == null) throw new ApiException(400, "Missing required parameter 'tokenId' when calling GetTokenIdOwners");

			var postBody = new Dictionary<String, String>();
			var queryParams = new Dictionary<String, String>();
			var headerParams = new Dictionary<String, String>();
			var formParams = new Dictionary<String, String>();
			var fileParams = new Dictionary<String, FileParameter>();

			var path = "/nft/{address}/{token_id}/owners";
			path = path.Replace("{format}", "json");
			path = path.Replace("{" + "address" + "}", ApiClient.ParameterToString(address));			path = path.Replace("{" + "token_id" + "}", ApiClient.ParameterToString(tokenId));
			queryParams.Add("chain", ApiClient.ParameterToHex((long)chain));
			if(format != null) queryParams.Add("format", ApiClient.ParameterToString(format));
			if(offset != null) queryParams.Add("offset", ApiClient.ParameterToString(offset));
			if(limit != null) queryParams.Add("limit", ApiClient.ParameterToString(limit));

			// Authentication setting, if any
			String[] authSettings = new String[] { "ApiKeyAuth" };

			string bodyData = postBody.Count > 0 ? JsonConvert.SerializeObject(postBody) : null;

			IRestResponse response = (IRestResponse)(await ApiClient.CallApi(path, Method.GET, queryParams, bodyData, headerParams, formParams, fileParams, authSettings));

			if (((int)response.StatusCode) >= 400)
				throw new ApiException((int)response.StatusCode, "Error calling GetTokenIdOwners: " + response.Content, response.Content);
			else if (((int)response.StatusCode) == 0)
				throw new ApiException((int)response.StatusCode, "Error calling GetTokenIdOwners: " + response.ErrorMessage, response.ErrorMessage);

			return (NftOwnerCollection)ApiClient.Deserialize(response.Content, typeof(NftOwnerCollection), response.Headers);
		}
		/// <summary>
		/// Gets the transfers of the tokens matching the given parameters
		/// </summary>
		/// <param name="address">Address of the contract</param>
		/// <param name="tokenId">The id of the token</param>
		/// <param name="chain">The chain to query</param>
		/// <param name="format">The format of the token id</param>
		/// <param name="offset">offset</param>
		/// <param name="limit">limit</param>
		/// <param name="order">The field(s) to order on and if it should be ordered in ascending or descending order. Specified by: fieldName1.order,fieldName2.order. Example 1: "block_number", "block_number.ASC", "block_number.DESC", Example 2: "block_number and contract_type", "block_number.ASC,contract_type.DESC"</param>
		/// <param name="cursor">The cursor returned in the last response (for getting the next page)
		/// </param>
		/// <returns>Returns a collection of NFT transfers</returns>
		public async Task<NftTransferCollection> GetWalletTokenIdTransfers (string address, string tokenId, ChainList chain, string format=null, int? offset=null, int? limit=null, string order=null, string cursor=null)
		{

			// Verify the required parameter 'address' is set
			if (address == null) throw new ApiException(400, "Missing required parameter 'address' when calling GetWalletTokenIdTransfers");

			// Verify the required parameter 'tokenId' is set
			if (tokenId == null) throw new ApiException(400, "Missing required parameter 'tokenId' when calling GetWalletTokenIdTransfers");

			var postBody = new Dictionary<String, String>();
			var queryParams = new Dictionary<String, String>();
			var headerParams = new Dictionary<String, String>();
			var formParams = new Dictionary<String, String>();
			var fileParams = new Dictionary<String, FileParameter>();

			var path = "/nft/{address}/{token_id}/transfers";
			path = path.Replace("{format}", "json");
			path = path.Replace("{" + "address" + "}", ApiClient.ParameterToString(address));			path = path.Replace("{" + "token_id" + "}", ApiClient.ParameterToString(tokenId));
			queryParams.Add("chain", ApiClient.ParameterToHex((long)chain));
			if(format != null) queryParams.Add("format", ApiClient.ParameterToString(format));
			if(offset != null) queryParams.Add("offset", ApiClient.ParameterToString(offset));
			if(limit != null) queryParams.Add("limit", ApiClient.ParameterToString(limit));
			if(order != null) queryParams.Add("order", ApiClient.ParameterToString(order));
			if(cursor != null) queryParams.Add("cursor", ApiClient.ParameterToString(cursor));

			// Authentication setting, if any
			String[] authSettings = new String[] { "ApiKeyAuth" };

			string bodyData = postBody.Count > 0 ? JsonConvert.SerializeObject(postBody) : null;

			IRestResponse response = (IRestResponse)(await ApiClient.CallApi(path, Method.GET, queryParams, bodyData, headerParams, formParams, fileParams, authSettings));

			if (((int)response.StatusCode) >= 400)
				throw new ApiException((int)response.StatusCode, "Error calling GetWalletTokenIdTransfers: " + response.Content, response.Content);
			else if (((int)response.StatusCode) == 0)
				throw new ApiException((int)response.StatusCode, "Error calling GetWalletTokenIdTransfers: " + response.ErrorMessage, response.ErrorMessage);

			return (NftTransferCollection)ApiClient.Deserialize(response.Content, typeof(NftTransferCollection), response.Headers);
		}
	}
}
