let button = document.getElementsByClassName("clicker_button")[0]
let counter = document.getElementsByClassName("clicker_counter")[0]
let counterFloat = 0
let cpsCounter = document.getElementsByClassName("cps_counter")[0]

let baseUrl = document.location.origin 
let userClickCoefficient
let userPassiveCoefficient
let row = document.getElementsByClassName("item_list")[0]


reloadPageBackUp()
genCards()
// intervalAutoSave(4000)

button.addEventListener("click", ActionClick, false)

// window.addEventListener("focus", async function(event) 
// { 
//     setMoneySite((parseInt(getMoneySite()) + buffer).toFixed(0))
//     buffer = 0
// }, false);



async function asyncRequest(method, url, bode = null) {
    console.log(url)
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
    let lUserClickCoefficient = localStorage.getItem("clickCoefficient")
    let lUserPassiveCoefficient = localStorage.getItem("passiveCoefficient")
    let lUserMoney = localStorage.getItem("money")
    let lUserSaveMoney = localStorage.getItem("saveMoney")

    if (lUserClickCoefficient === null || lUserPassiveCoefficient === null || lUserMoney === null || lUserSaveMoney == null) {
        let user = await getUser()
        localStorage.setItem("clickCoefficient", user["clickCoefficient"])
        localStorage.setItem("passiveCoefficient", user["passiveCoefficient"])
        localStorage.setItem("money", user["money"])
        localStorage.setItem("saveMoney", user["money"])

        
        userClickCoefficient = user["clickCoefficient"]
        userPassiveCoefficient = user["passiveCoefficient"]
        
        counterFloat = Number(user["money"])
        counter.innerText = user["money"]
        cpsCounter.innerHTML = user["passiveCoefficient"] + " cps"
    }
    else {
        userClickCoefficient = lUserClickCoefficient
        userPassiveCoefficient = lUserPassiveCoefficient

        counterFloat = Number(lUserMoney)
        counter.innerText = lUserMoney
        cpsCounter.innerHTML = userPassiveCoefficient + " cps"
    }
}


async function ActionClick() {
    let m = localStorage.getItem('money');
    counterFloat+=Number(localStorage.getItem("clickCoefficient"))
    localStorage.setItem('money', Number(m === null ? 0 : m) + Number(localStorage.getItem("clickCoefficient")));
}
