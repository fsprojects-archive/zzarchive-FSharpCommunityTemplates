namespace $safeprojectname$.Communication

type IRouter<'a> =
    abstract Send: 'a -> unit

module Commands =
    open System

    let IssuedByKey = "IssuedBy"

    type Issuer =
        | System
        | User of Username: string

        override this.ToString() =
            match this with
            | System -> "DICE"
            | User name -> name

    type CommandData =
        {
            EntityId: Guid
            Issuer: Issuer
        }

    type Command =
        {
            Data: CommandData
            Body: obj
        }

module CommandBus =
    open Commands
    
    let createCommandRouter handlers =
        let event = Event<Command>()
        handlers |> List.iter (fun h -> event.Publish |> Event.add h)

        let agent = MailboxProcessor.Start(fun inbox -> 
            let rec loop() = async {
                let! cmd = inbox.Receive()
                cmd |> event.Trigger
                return! loop() }
            loop())

        { new IRouter<Command> with
            member __.Send (cmd: Command) =
                agent.Post cmd }