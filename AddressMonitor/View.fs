module View
open Suave.Html
open Suave.Xml
open Suave.Form

let divId id interior = divAttr["id", id] interior
let aHref link label = tag "a" ["href", link] label
let h1 = tag "h1" []

let index container = xmlToString <|
                        html [
                            head [
                                title "Address Monitor"
                            ]

                            body [
                                divId "header"[
                                    h1 (aHref Path.home (text "Address Monitor - Home"))
                                ]

                                divId "main" container

                                divId "footer" [
                                    text "built with "
                                    aHref "http://fsharp.org" (text "F#")
                                    text " and "
                                    aHref "http://suave.io" (text "Suave.IO")
                                ]            
                            ]
                        ]

let home = [ text "Home" ]

let notFound = [
    h1 (text "Page not found")

    p [
        text "Could not find the requested page"
    ]

    p [
        text "Back to "
        aHref Path.home (text "Home")
    ]
]