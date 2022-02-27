
/* ----------------------------------------------------------------------------------------- */
/*    The file containing the functions of filling the page with data when loading the page, */ 
/*    the store with build, the user profile and the list of leaders.                        */
/* ----------------------------------------------------------------------------------------- */

/* ------------------------------ HTML ELEMENTS ----------------------------- */
let content = document.getElementById("content") /** HTML Content Block */
let row = document.getElementsByClassName("item_list")[0] /** element which contain perks cards */
let button = document.getElementsByClassName("clicker_button")[0] /** main button for click */
let cpsCounter = document.getElementsByClassName("cps_counter")[0] /** How much money does a user get per second */
let counter = document.getElementsByClassName("clicker_counter")[0] /** users clicks counter to send them to the server for saving */
/* ------------------------------------ / ----------------------------------- */

/* ----------------------------- USER INFORMATION ---------------------------- */
let counterFloat
let userClickCoefficient
let userPassiveCoefficient
let baseUrl = document.location.origin
let lUserId = localStorage.getItem("Id")
let lUserMoney = localStorage.getItem("money")
let lUserNickname = localStorage.getItem("nickname")
let lUserClickCoefficient = localStorage.getItem("clickCoefficient")
let lUserPassiveCoefficient = localStorage.getItem("passiveCoefficient")
/* ------------------------------------ / ----------------------------------- */


init()


async function init() {
    /* ------------------------ Initialize main elements ------------------------ */
    await genCards() /** Generate perks cards in store with data from api */
    await loadUserValues() /** Call function to load users values into local storage from api */
    await setDataToProfileCard() /** Uploading data to the user profile */
    /* ------------------------------------ / ----------------------------------- */
}

/** Function load user info to site */
async function loadUserValues() {
    let lDataAvailability = lUserClickCoefficient && lUserPassiveCoefficient && lUserMoney && lUserNickname && lUserId
    if (!lDataAvailability) {
        let user = await getUserInformation()

        localStorage.setItem("Id", user["id"])
        localStorage.setItem("money", user["money"])
        localStorage.setItem("nickname", user["nickname"])
        localStorage.setItem("clickCoefficient", user["clickCoefficient"])
        localStorage.setItem("passiveCoefficient", user["passiveCoefficient"])

        counter.innerText = user["money"]
        counterFloat = Number(user["money"]) /** counterFloat contain money with decimal places */
        userClickCoefficient = user["clickCoefficient"]
        userPassiveCoefficient = user["passiveCoefficient"]
        cpsCounter.innerHTML = user["passiveCoefficient"] + " cps"
    }
    else {
        counter.innerText = lUserMoney
        counterFloat = Number(lUserMoney) /** counterFloat contain money with decimal places */
        userClickCoefficient = lUserClickCoefficient
        userPassiveCoefficient = lUserPassiveCoefficient
        cpsCounter.innerHTML = userPassiveCoefficient + " cps"
    }
}