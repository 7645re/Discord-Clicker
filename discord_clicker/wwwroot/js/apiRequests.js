/* -------------------------------------- */
/*    The file containing api requests    */
/* -------------------------------------- */


/** Ajax request */
async function asyncRequest(method, url, bode = null) {
    return fetch(url).then(response => {
        return response.json()
    })
}

// async function asyncRequest(method, url) {
//   return fetch(url).then(response => {
//       if (response.ok) {
//         return response.json().then(response => ({ response }));
//       }
//       return response.json().then(error => ({ error }));
//     })
//   ;
// }


/** Function for getting user information */
async function getUserInformation() {
    let result;
    await asyncRequest('GET', "/api/getUserInformation")
        .then(data => { result = data["response"] })
        .catch(err => console.log(err))
    console.log(result)
    return result
  }
  
/** Function for getting a list of buildings */
async function getBuildsList() {
    let result
    await asyncRequest('GET', "/api/getBuildsList")
        .then(data => { result = data })
        .catch(err => console.log(err))
    console.log(result)
    return result
}
/** Function for buying a building by ID */
async function buyBuild(buildId) {
  let result
  await asyncRequest('GET', `api/buyBuild?buildId=${buildId}&money=${localStorage.getItem("money")}`)
  .then(data => { result = data })
  .catch(err => console.log(err))
  console.log(result)
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