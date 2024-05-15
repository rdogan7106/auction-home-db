
Medverkande: 

Rahman, André och Veton 

 

Githubkonto: 

hjulstromandre 	// Andre 

rdogan7106		// Rahman 

Barileva		//Veton 

 

EndpointS: 

app.MapGet("/auctions/{itemId}/bidHistory", 	// André 

AuctionManager.GetBidHistoryForAuction);	// André 

app.MapPost("/bids", AuctionManager.AddBid); 	// André 

app.MapPost("/users", Users.AddUser);		//  Rahman 

app.MapGet("/users", Users.All); 			// Rahman, André och Veton 

app.MapDelete("/users/{userID}", Users.DeleteUser); 	// Rahman och Veton 

app.MapPut("/users/{userID}", Users.UpdateUser);	// Rahman 

app.MapGet("/auctions", AuctionManager.GetAllItems);	// Rahman 

app.MapPost("/auctions", AuctionManager.AddItem);	// Rahman och Veton 

app.MapDelete("/auctions/{ItemID}", AuctionManager.DeleteItem);	// Rahman 

app.MapPut("/auctions/{ItemID}", AuctionManager.UpdateAuction); // Rahman 

app.MapGet("/auctions/{status}", AuctionManager.GetSoldItems);	// Veton 
