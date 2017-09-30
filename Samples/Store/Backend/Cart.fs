﻿module Backend.Cart

open Domain

type Service(createStream) =
    let codec = Foldunk.EventSum.generateJsonUtf8SumEncoder<Cart.Events.Event>
    let streamName (id: CartId) = sprintf "Cart-%s" id.Value
    let handler id = Cart.Handler(streamName id |> createStream codec)

    member __.Decide (log : Serilog.ILogger) (cartId : CartId) decide =
        let handler = handler cartId
        handler.Decide log decide

    member __.Read (log : Serilog.ILogger) (cartId : CartId) =
        let handler = handler cartId
        handler.Read log