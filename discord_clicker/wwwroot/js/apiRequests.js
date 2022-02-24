/* -------------------------------------------------------------------------- */
/*    This file is responsible for interacting with the server via the api    */
/* -------------------------------------------------------------------------- */

/* ------------------------------ HTML ELEMENTS ----------------------------- */
let button = document.getElementsByClassName("clicker_button")[0] /** main button for click */
let counter = document.getElementsByClassName("clicker_counter")[0] /** users clicks counter to send them to the server for saving */
let cpsCounter = document.getElementsByClassName("cps_counter")[0] /** How much money does a user get per second */
let row = document.getElementsByClassName("item_list")[0] /** element which contain perks cards */
let content = document.getElementById("content")
/* ------------------------------------ / ----------------------------------- */

/* ----------------------------- USER'S VAR'S ---------------------------- */
let baseUrl = document.location.origin /** url for ajax request */
let userClickCoefficient
let userPassiveCoefficient
let counterFloat
let lUserClickCoefficient = localStorage.getItem("clickCoefficient")
let lUserPassiveCoefficient = localStorage.getItem("passiveCoefficient")
let lUserMoney = localStorage.getItem("money")
let lUserNickname = localStorage.getItem("nickname")
let lUserId = localStorage.getItem("Id")
/* ------------------------------------ / ----------------------------------- */


/* ------------------------ Initialize main elements ------------------------ */
loadUserValues() /** Call function to load users values into local storage from api */
genCards() /** Generate perks cards in store with data from api */
setDataToProfileCard()
/* ------------------------------------ / ----------------------------------- */

/** Ajax request */
async function asyncRequest(method, url, bode = null) {
    console.log(url)
    return fetch(url).then(response => {
        return response.json()
    })
}

/** Function that return user info from api */
async function getUser() {
    let result;
    await asyncRequest('GET', "/api/getuserinformation")
        .then(data => { result = data["user"] })
        .catch(err => console.log(err))
    console.log(result)
    return result
}

/** Function load user info to site */
async function loadUserValues() {
    let lDataAvailability = lUserClickCoefficient && lUserPassiveCoefficient && lUserMoney && lUserNickname && lUserId
    if (!lDataAvailability) {
        let user = await getUser()
        localStorage.setItem("clickCoefficient", user["clickCoefficient"])
        localStorage.setItem("passiveCoefficient", user["passiveCoefficient"])
        localStorage.setItem("money", user["money"])
        localStorage.setItem("nickname", user["nickname"])
        localStorage.setItem("Id", user["id"])
        userClickCoefficient = user["clickCoefficient"]
        userPassiveCoefficient = user["passiveCoefficient"]
        
        /** counterFloat contain money with decimal places */
        counterFloat = Number(user["money"])
        counter.innerText = user["money"]
        cpsCounter.innerHTML = user["passiveCoefficient"] + " cps"
    }
    else {
        userClickCoefficient = lUserClickCoefficient
        userPassiveCoefficient = lUserPassiveCoefficient

        /** counterFloat contain money with decimal places */
        counterFloat = Number(lUserMoney)
        counter.innerText = lUserMoney
        cpsCounter.innerHTML = userPassiveCoefficient + " cps"
    }
}

