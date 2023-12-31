open Microsoft.AspNetCore.Builder
open Giraffe
open Giraffe.ViewEngine
open Microsoft.AspNetCore.Http

let _hxGet = attr "hx-get"
let _hxTarget = attr "hx-target"
let _hxSwap = attr "hx-swap"

let tableView next (ctx: HttpContext) =
    task {
        let view =
            table [] [
                thead [] [
                    tr [] [
                        th [] [ str "Id" ]
                        th [] [ str "Name" ]
                    ]
                ]
                tbody [] [
                    tr [] [
                        td [] [ str "1" ]
                        td [] [ str "chameleon" ]
                    ]
                    tr [] [
                        td [] [ str "2" ]
                        td [] [ str "octopus" ]
                    ]
                    tr [] [
                        td [] [ str "3" ]
                        td [] [ str "ray" ]
                    ]
                ]
            ]

        return! htmlView view next ctx
    }

let simpleView next (ctx: HttpContext) =
    task {
        //<script src="https://unpkg.com/htmx.org@1.9.10" integrity="sha384-D1Kt99CQMDuVetoL1lrYwg5t+9QdHe7NLX/SoJYkXDFfX37iInKRy5xLSi8nO7UC" crossorigin="anonymous"></script>
        let view =
            html [] [
                head [] [
                    script [ _src "https://unpkg.com/htmx.org@1.9.10" ] []
                ]
                body [] [
                    h1 [] [ str "View users" ]
                    p [ _hxGet "/users"; _hxTarget "#showUsers"; _hxSwap "outerHtml" ] [
                        str "Test"
                    ]
                    div [ _id "showUsers" ] []
                ]
            ]

        return! htmlView view next ctx
    }

let giraffe =
    choose [ GET >=> route "/" >=> simpleView
             GET >=> route "/users" >=> tableView ]

let builder = WebApplication.CreateBuilder()
builder.Services.AddGiraffe() |> ignore

let app = builder.Build()
app.UseGiraffe giraffe
app.Run()
