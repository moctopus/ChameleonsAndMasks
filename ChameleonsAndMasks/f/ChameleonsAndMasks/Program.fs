module ChameleonsAndMasks.Program

open Falco
open Falco.Routing
open Falco.HostBuilder
open Falco.Markup.Svg
open Falco.Markup
open Falco.Htmx

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
                Elem.div [ Hx.get "/users"; Hx.target (Hx.Target.css "#swapDiv"); Hx.swap Hx.Swap.outerHTML ] [
                    Elem.p [] [ Text.raw "View users" ]
                ]
                Elem.div [ Hx.get "/posts/1"; Hx.target (Hx.Target.css "#swapDiv"); Hx.swap Hx.Swap.outerHTML ] [
                    Elem.p [] [ Text.raw "Test" ]
                ]
                Elem.div [ Attr.id "swapDiv" ] []
            ]
        ]

    Response.ofHtml html

[<EntryPoint>]
let main args =
    webHost args {
        endpoints [
            get "/" htmlHandler
            get "/users" tableView
        ]
    }
    0