module ChameleonsAndMasks.Program

open Falco
open Falco.Routing
open Falco.HostBuilder
open Falco.Markup.Svg
open Falco.Markup
open Falco.Htmx

let getPost id =
    match id with
    | "1" -> "some stuff"
    | "2" -> "some other stuff"
    | "3" -> "what was I doing?"
    | _ -> "error"

let postsView : HttpHandler = fun ctx ->
    let r = Request.getRoute ctx
    let id = r.GetString "id"
    let view = 
        Elem.div [] [
                Elem.h2 [] [ Text.raw id ]
                Elem.p [] [ Text.raw <| getPost id ]
                Elem.a [ Hx.get $"/posts/{(int id) + 1}"; Hx.target (Hx.Target.css "#swapDiv"); Hx.swap Hx.Swap.innerHTML ] [
                    Text.raw "Next"
                ]
            ]
    
    Response.ofHtml view ctx

let tableView : HttpHandler = 
    let view =
            Elem.table [] [
                Elem.thead [] [
                    Elem.tr [] [
                        Elem.th [] [ Text.raw "Id" ]
                        Elem.th [] [ Text.raw "Name" ]
                    ]
                ]
                Elem.tbody [] [
                    Elem.tr [] [
                        Elem.td [] [ Text.raw "1" ]
                        Elem.td [] [ Text.raw "chameleon" ]
                    ]
                    Elem.tr [] [
                        Elem.td [] [ Text.raw "2" ]
                        Elem.td [] [ Text.raw "octopus" ]
                    ]
                    Elem.tr [] [
                        Elem.td [] [ Text.raw "3" ]
                        Elem.td [] [ Text.raw "ray" ]
                    ]
                ]
            ]
    Response.ofHtml view

let htmlHandler : HttpHandler =
    let html =
        Elem.html [ Attr.lang "en" ] [
            Elem.head [] [
                Elem.script [ Attr.src "https://unpkg.com/htmx.org@1.9.10" ] []
            ]
            Elem.body [] [
                Elem.h1 [] [ Text.raw "View users" ]
                Elem.div [ Hx.get "/users"; Hx.target (Hx.Target.css "#swapDiv"); Hx.swap Hx.Swap.innerHTML ] [
                    Elem.p [] [ Text.raw "View users" ]
                ]
                Elem.div [ Hx.get "/posts/1"; Hx.target (Hx.Target.css "#swapDiv"); Hx.swap Hx.Swap.innerHTML ] [
                    Elem.p [] [ Text.raw "Posts" ]
                ]
                Elem.div [ Attr.id "swapDiv" ] []
            ]
        ]

    Response.ofHtml html

[<EntryPoint>]
let main args =
    webHost args {
        endpoints [
            get "/posts/{id:int}" postsView
            get "/users" tableView
            get "/" htmlHandler
        ]
    }
    0