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

/* ----------------------------- USER'S COUNTERS ---------------------------- */
let baseUrl = document.location.origin /** url for ajax request */
let userClickCoefficient
let userPassiveCoefficient
let counterFloat
/* ------------------------------------ / ----------------------------------- */


/* ------------------------ Initialize main elements ------------------------ */
loadUserValues() /** Call function to load users values into local storage from api */
genCards() /** Generate perks cards in store with data from api */
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
    await asyncRequest('GET', "/getuserinformation")
        .then(data => { result = data["user"] })
        .catch(err => console.log(err))
    return result
}

/** Function load user info to site */
async function loadUserValues() {
    /** Variables for data from local storage*/
    let lUserClickCoefficient = localStorage.getItem("clickCoefficient")
    let lUserPassiveCoefficient = localStorage.getItem("passiveCoefficient")
    let lUserMoney = localStorage.getItem("money")

    if (lUserClickCoefficient === null || lUserPassiveCoefficient === null || lUserMoney === null) {
        let user = await getUser()
        localStorage.setItem("clickCoefficient", user["clickCoefficient"])
        localStorage.setItem("passiveCoefficient", user["passiveCoefficient"])
        localStorage.setItem("money", user["money"])

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

