//const connection = new signalR.HubConnectionBuilder()
//    .withUrl("/notificacoes")
//    .build();

const connection =
    new signalR.HubConnectionBuilder()
        .withUrl(
            "/notificacoes?usuarioId=11111111-1111-1111-1111-111111111111"
        )
        .build();


connection.on("ReceberNotificacao",
    function (mensagem) {
        alert(mensagem);
    }
);



connection.start()
    .then(function () {
        console.log(
            "SignalR conectado"
        );

    })
    .catch(function (err) {

        console.error(err);

    });