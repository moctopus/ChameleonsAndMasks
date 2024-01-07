open Microsoft.AspNetCore.Builder
open Giraffe
open Giraffe.ViewEngine
open Microsoft.AspNetCore.Http

let _hxGet = attr "hx-get"
let _hxTarget = attr "hx-target"
let _hxSwap = attr "hx-swap"

let getPost id =
    match id with
    | "1" -> "some stuff"
    | "2" -> "some other stuff"
    | _ -> "error"

let postsView (id: string) next (ctx: HttpContext) =
    task {
        let view =
            div [] [
                h2 [] [ str id ]
                p [] [ str <| getPost id ]
                a [ _hxGet $"/posts/{(int id) + 1}"; _hxTarget "#swapDiv"; _hxSwap "outerHtml" ] [
                    str "Next"
                ]
            ]

        return! htmlView view next ctx
    }

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
                    link [_rel "stylesheet"; _href "/css/bulma.min.css"]
                ]
                body [] [
                    h1 [] [ str "View users" ]
                    div [ _hxGet "/users"; _hxTarget "#swapDiv"; _hxSwap "outerHtml" ] [
                        p [] [ str "Test" ]
                    ]
                    div [ _hxGet "/posts/1"; _hxTarget "#swapDiv"; _hxSwap "outerHtml" ] [
                        p [] [ str "Test" ]
                    ]
                    div [ _id "swapDiv" ] []
                ]
            ]

        return! htmlView view next ctx
    }

let giraffe =
    choose [ GET >=> route "/" >=> simpleView
             GET >=> route "/users" >=> tableView
             GET >=> routef "/posts/%s" postsView ]

let builder = WebApplication.CreateBuilder()
builder.Services.AddGiraffe() |> ignore

let app = builder.Build()
app.UseStaticFiles()
app.UseGiraffe giraffe
app.Run()
