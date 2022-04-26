/* ----------------------------------------------------------------------------------------- */
/*    The file containing the functions of filling the page with data when loading the page, */ 
/*    the store with build, the user profile and the list of leaders.                        */
/* ----------------------------------------------------------------------------------------- */

/* ------------------------------ HTML ELEMENTS ----------------------------- */
let content = document.getElementById("content") /** HTML Content Block */
let builds_list = document.getElementsByClassName("builds_list")[0] /** element which contain builds cards */
let button = document.getElementsByClassName("clicker_button")[0] /** main button for click */
let cpsCounter = document.getElementsByClassName("cps_counter")[0] /** How much money does a user get per second */
let counter = document.getElementsByClassName("clicker_counter")[0] /** users clicks counter to send them to the server for saving */
/* ------------------------------------ / ----------------------------------- */

/* ----------------------------- USER INFORMATION ---------------------------- */
let counterFloat
let userClickCoefficient
let userPassiveCoefficient
let lUserId = localStorage.getItem("id")
let lUserRole = localStorage.getItem("role")
let lUserMoney = localStorage.getItem("money")
let lUserBuilds = localStorage.getItem("builds")
let lUserNickname = localStorage.getItem("nickname")
let lUserUpgrades = localStorage.getItem("upgrades")
let lUserAchievements = localStorage.getItem("achievements")
let lUserClickCoefficient = localStorage.getItem("clickCoefficient")
let lUserPassiveCoefficient = localStorage.getItem("passiveCoefficient")
/* ------------------------------------ / ----------------------------------- */

/* ----------------------------- ITEMS INFORMATION ---------------------------- */
let lBuilds = localStorage.getItem("buildsList")
let lUpgrades = localStorage.getItem("upgradesList")
let lAchievements = localStorage.getItem("achievementsList")
/* ------------------------------------ / ----------------------------------- */


init()


async function init() {
    /* ------------------------ Initialize main elements ------------------------ */
    await loadUser() /** Call function to load users values into local storage from api */
    await loadItems() /** Call function to load items to local storage from api */
    await genCards() /** Generate perks cards in store with data from api */
    // await setDataToProfileCard() /** Uploading data to the user profile */
    /* ------------------------------------ / ----------------------------------- */
}

/** Function load user info to site */
async function loadUser() {
    let lDataAvailability = lUserId && lUserRole && lUserMoney && lUserBuilds 
        && lUserNickname && lUserUpgrades && lUserAchievements && lUserClickCoefficient && lUserPassiveCoefficient
    if (!lDataAvailability) {
        let user = await getStats()
        for (key in user) {
            await localStorage.setItem(key, JSON.stringify(user[key]))
        }
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
async function loadItems() {
    let lDataAvailability = lBuilds && lUpgrades && lAchievements
    if (!lDataAvailability) {
        let builds = await getBuildsList()
        let upgrades = await getUpgradesList()
        let achievements = await getAchievementsList()
        localStorage.setItem("buildsList", JSON.stringify(builds));
        localStorage.setItem("upgradesList", JSON.stringify(upgrades));
        localStorage.setItem("achievementList", JSON.stringify(achievements));
    }
}