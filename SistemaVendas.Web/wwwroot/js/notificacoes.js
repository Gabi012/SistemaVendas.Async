const usuarioId = "11111111-1111-1111-1111-111111111111";

// -------------------------
// SignalR
// -------------------------

const connection = new signalR.HubConnectionBuilder()
    .withUrl(`/hubs/notificacoes?usuarioId=${usuarioId}`)
    .withAutomaticReconnect()
    .build();

connection.on("ReceberNotificacao", function (mensagem) {

    console.log(mensagem);

    atualizarContador();

    adicionarNotificacaoNaTela(mensagem);

});

connection.start()
    .then(() => {

        console.log("SignalR conectado");

        atualizarContador();

    })
    .catch(err => console.error(err));


// -------------------------
// Contador
// -------------------------

async function atualizarContador() {
    console.log("Entrou em atualizarContador")
    const response =
        await fetch("/Notificacoes/Contador");

    const total =
        await response.json();

    const contador =
        document.getElementById("contadorNotificacoes");

    if (contador)
        contador.innerText = total;

}


// -------------------------
// Lista
// -------------------------

function adicionarNotificacaoNaTela(mensagem) {

    const lista =
        document.getElementById("listaNotificacoes");

    if (!lista)
        return;

    const card =
        document.createElement("div");

    card.className = "card mb-2";

    card.innerHTML = `
        <div class="card-body">

            <strong>${mensagem}</strong>

            <br>

            <small>${new Date().toLocaleString()}</small>

        </div>
    `;

    lista.prepend(card);

}

document.addEventListener("click", async function (e) {

    if (!e.target.classList.contains("marcar-lida"))
        return;

    const card = e.target.closest(".notificacao");

    const id = card.dataset.id;

    const response = await fetch("/Notificacoes/MarcarComoLida?id=" + id,
        {
            method: "POST"
        });

    if (!response.ok)
        return;

    card.classList.remove("nao-lida");
    card.classList.add("lida");

    e.target.remove();

    atualizarContador();

});