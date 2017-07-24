module View
open Suave.Html
open Suave.Xml
open Suave.Form

let divId id = divAttr["id", id]

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