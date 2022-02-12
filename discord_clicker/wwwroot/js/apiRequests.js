let button = document.getElementsByClassName("clicker_button")[0]
let htmlCounter = document.getElementsByClassName("clicker_counter")[0]
let content = document.getElementById("content")
let cpsCounter = document.getElementsByClassName("cps_counter")[0]
let baseUrl = document.location.origin 
let click = 0
let userCoefficient;
let userPassiveCoefficient;
var buffer = 0
var dBuffer = 0
var requestCounter = 0
let row = document.getElementsByClassName("row")[0]


reloadPageBackUp()
genCards()
intervalAutoSave(4000)

button.addEventListener("click", ActionClick, false)

window.addEventListener("focus", async function(event) 
{ 
    setMoneySite((parseInt(getMoneySite()) + buffer).toFixed(0))
    buffer = 0
}, false);



async function asyncRequest(method, url, bode = null) {
    requestCounter++
    console.log(requestCounter)
    return fetch(url).then(response => {
        return response.json()
    })
}

async function setMoneyDB(money) {
    let result;
    await asyncRequest('GET', `setmoney?money=${money}`)
        .then(data => { result = data })
        .catch(err => console.log(err))
    return result

}

async function getUser() {
    let result;
    await asyncRequest('GET', "/getuserinformation")
        .then(data => { result = data["user"] })
        .catch(err => console.log(err))
    return result

}


async function reloadPageBackUp() {
    let user = await getUser()
    userCoefficient = user["clickCoefficient"]
    userPassiveCoefficient = user["passiveCoefficient"]
    let moneyDb = user["money"]
    setMoneySite(moneyDb)
    cpsCounter.innerHTML = userPassiveCoefficient + " cps"
    return user
}


async function ActionClick() {
    click++
    setMoneySite(parseInt(getMoneySite()) + userCoefficient)
}
