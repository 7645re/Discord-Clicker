/* -------------------------------------- */
/*    The file containing requests to api */
/* -------------------------------------- */
async function asyncRequest(method, url, body = null, headers = null) {
    switch (method) {
        case "POST":
            return fetch(url, {
                method: method,
                body: JSON.stringify(body),
                headers: headers == null ? {"Content-Type": "application/json"} : headers
            }).then(response => {
                return response.json()
            })
        case "GET":
            return fetch(url, {
                method: method,
            }).then(response => {
                return response.json()
            })
    }
}
async function getUserAsyncRequest() {
    let result;
    await asyncRequest('GET', "/stats")
        .then(data => {
            result = data
        })
        .catch(err => console.log(err))
    return result
}
async function getBuildsListAsyncRequest() {
    let result
    await asyncRequest('GET', "builds")
        .then(data => {
            result = data
        })
        .catch(err => console.log(err))
    return result
}
async function getUpgradesListAsyncRequest() {
    let result
    await asyncRequest('GET', "upgrades")
        .then(data => {
            result = data
        })
        .catch(err => console.log(err))
    return result
}
async function getAchievementsListAsyncRequest() {
    let result
    await asyncRequest('GET', "achievements")
        .then(data => {
            result = data
        })
        .catch(err => console.log(err))
    return result
}
async function createBuildAsyncRequest(itemCreateModel) {
    let result
    await asyncRequest('POST', "/createBuild", itemCreateModel)
        .then(data => {
            result = data
        })
        .catch(err => console.log(err))
    return result
}
async function createUpgradeAsyncRequest(itemCreateModel) {
    let result
    await asyncRequest('POST', "/createUpgrade", itemCreateModel)
        .then(data => {
            result = data
        })
        .catch(err => console.log(err))
    return result
}
async function createAchievementAsyncRequest(itemCreateModel) {
    let result
    await asyncRequest('POST', "/createAchievement", itemCreateModel)
        .then(data => {
            result = data
        })
        .catch(err => console.log(err))
    return result
}
async function buyBuildAsyncRequest(id, money) {
    let result
    await asyncRequest('GET', "/buyBuild?id=" + id + "&money=" + money)
        .then(data => {
            result = data
        })
        .catch(err => console.log(err))
    return result
}


// const rainContainer = document.querySelector(".rain-container");

// // background Colors for the raindrop
// const background = [
//   "linear-gradient(transparent, aqua)",
//   "linear-gradient(transparent, red)",
//   "linear-gradient(transparent, limegreen)",
//   "linear-gradient(transparent, white)",
//   "linear-gradient(transparent, yellow)"
// ];

// const amount = 5; // amount of raindops
// let i = 0;
// function getRandomArbitrary(min, max) {
//     return Math.random() * (max - min) + min;
//   }
// // Looping and creating the raindrop then adding to the rainContainer
// while (i < amount) {
//   //  Creating and Element
//   const drop = document.createElement("img");
//   drop.src = "images/PerksImg/Balance Badge.svg"
//   drop.className = "rainElement"
//   //   CSS Properties for raindrop
//   const raindropProperties = {
//     // width: Math.random() * 5 + "px",
//     positionX: Math.floor(getRandomArbitrary(0,1) * window.innerWidth) + "px",
//     delay: getRandomArbitrary(1,2) * -20 + "s",
//     duration: getRandomArbitrary(1,2) * 5 + "s",
//     bg: background[Math.floor(getRandomArbitrary(1,2) * background.length)],
//     opacity: Math.random() + 0.2
//   };

//   //   Setting Styles for raindrop
//   drop.style.width = raindropProperties.width;
//   drop.style.left = raindropProperties.positionX;
//   drop.style.animationDelay = raindropProperties.delay;
//   drop.style.animationDuration = raindropProperties.duration;
// //   drop.style.background = raindropProperties.bg;
//   drop.style.opacity = raindropProperties.opacity;

//   //   Appending the raindrop in the raindrop container
//   rainContainer.appendChild(drop);
//   i++;
// }