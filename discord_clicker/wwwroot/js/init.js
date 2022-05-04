/* ----------------------------------------------------------------------------------------- */
/*    The file containing the functions of filling the page with data when loading the page, */ 
/*    the store with build, the user profile and the list of leaders.                        */
/* ----------------------------------------------------------------------------------------- */

let content = document.getElementsByClassName("main")[0] /** HTML Content Block */
let builds_list = document.getElementsByClassName("builds_list")[0] /** element which contain builds cards */
let button = document.getElementsByClassName("clicker_button")[0] /** main button for click */
let cpsCounter = document.getElementsByClassName("cps_counter")[0] /** How much money does a user get per second */
let counter = document.getElementsByClassName("clicker_counter")[0] /** users clicks counter to send them to the server for saving */
let builds_button = document.getElementsByClassName("btn-builds")[0]
let upgrades_button = document.getElementsByClassName("btn-upgrades")[0]
let profile_button = document.getElementsByClassName("btn-profile")[0]
let leaderboard_button = document.getElementsByClassName("btn-leaderboard")[0]

let lUser
let lBuilds 
let lUpgrades 
let lAchievements 

init()

async function init() {
    await loadUserAsync() /** Call function to load users values into local storage from api */
    await loadItemsAsync() /** Call function to load items to local storage from api */
    
    await genBuildsCardsAsync() /** Generate perks cards in store */
    // await updater() /** Animation of increasing the amount of money */ 
    
    button.addEventListener("click", ActionClick)
    profile_button.addEventListener("click", setDataToProfileCard)
}

/** Function load user info to site */
async function loadUserAsync() {
    lUser = JSON.parse(localStorage.getItem("user"))
    if (!lUser) {
        lUser = await getUserAsyncRequest()
        localStorage.setItem("user", JSON.stringify(lUser))
    }
}
async function loadItemsAsync() {
    if (!(lBuilds && lUpgrades && lAchievements)) {
        lBuilds = await getBuildsListAsyncRequest()
        lUpgrades = await getUpgradesListAsyncRequest()
        lAchievements = await getAchievementsListAsyncRequest()
        
        localStorage.setItem("buildsList", JSON.stringify(lBuilds));
        localStorage.setItem("upgradesList", JSON.stringify(lUpgrades));
        localStorage.setItem("achievementList", JSON.stringify(lAchievements));
    }
}