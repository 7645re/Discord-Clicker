async function minimazeNum(value) {
    return  new Intl.NumberFormat( 'en-US', { maximumFractionDigits: 1,notation: "compact" , compactDisplay: "short" }).format(String(value))
}   
//
// async function buyBuild(evt) {
//     let result
//     let userMoney
//     let status = result["status"]
//     let buildId = evt.currentTarget.getAttribute("buildId")
//     if (status == "ok") {
//
//         userMoney = result["money"]
//         userClickCoefficient = result["clickCoefficient"]
//         userPassiveCoefficient = result["passiveCoefficient"]
//
//         let buildCostValue = result["cost"]
//         let buyedBuildCount = result["buyedBuildCount"]
//         let buildCostSpan = document.createElement("span")
//         let buildPurchasedValue = result["buyedBuildCount"]+1
//         let buildPurchasedSpan = document.createElement("span")
//         let buildCost = document.getElementById(buildId+"-cost")
//         let buildPurchased = document.getElementById(buildId+"-lvl")
//         let profileBuildsHTML = document.getElementsByClassName("builds")[0]
//
//         counterFloat = userMoney
//         buildPurchased.innerHTML = ""
//         buildCost.children[1].remove()
//         buildPurchasedSpan.innerText = "Lvl " + buildPurchasedValue
//         buildCostSpan.innerText = minimazeNum(buildPurchasedValue * buildCostValue)
//
//         counter.innerText = userMoney
//         buildCost.appendChild(buildCostSpan)
//         buildPurchased.appendChild(buildPurchasedSpan)
//         cpsCounter.innerHTML =  userPassiveCoefficient + " cps" 
//
//         localStorage.setItem("money", userMoney)
//         localStorage.setItem("passiveCoefficient", userPassiveCoefficient)
//         localStorage.setItem("clickCoefficient", userClickCoefficient)
//
//         if (buyedBuildCount == 1) {
//             profileBuildsHTML.innerHTML += `<img class="profile-badge" src="images/buildsImg/${result["name"]}.svg"/>`
//         }
//     }
//     if (status == "cheat") {
//         userMoney = result["money"]
//         userClickCoefficient = result["clickCoefficient"]
//         userPassiveCoefficient = result["passiveCoefficient"]
//
//         counterFloat = userMoney
//         counter.innerText = userMoney
//
//         localStorage.setItem("money", userMoney)
//         localStorage.setItem("clickCoefficient", userClickCoefficient)
//         localStorage.setItem("passiveCoefficient", userPassiveCoefficient)
//     }
// }


async function genCards() {
    let builds = JSON.parse(lBuilds)
    for await (let build of builds) {
        //     strbuildCost = String(build.cost)
        //     strbuildPassiveCoefficient = String(build.passiveCoefficient)
        //     // build.passiveCoefficient = minimazeNum(strbuildPassiveCoefficient)
        //
        //     // newbuildCost = minimazeNum(buildsCount[build.id] > 0 ? build.cost * (buildsCount[build.id]+1) : build.cost)
        //
            builds_list.innerHTML += `
                        <div class="item-card col-sm-12 col-md-4">
                            <div class="build" id="build" buildid="${build.id}">
                                <div class="build-img" id="${build.id}-img"><img src="images/buildsImg/png/Balance Badge.png"  style="position: relative; left: 25%"/></div>
                                <div class="build-name" id="${build.id}-name"></div>
                                <div class="build-cost" id="${build.id}-cost">
                                    <img width="20" src="https://pnggrid.com/wp-content/uploads/2021/05/Black-and-white-Discord-Logo-1024x784.png"/>
                                    <span></span>
                                </div>
                                <div class="build-purchased" id="${build.id}-lvl"><span>Lvl </span></div>
                                <div class="build-passive" id="${build.id}-increase">Cps </div>
                            </div>
                        </div>`
        // }
        // const buildsCards = document.querySelectorAll('#build');
        // buildsCards.forEach(build => {
        //     build.addEventListener('click', buyBuild, false);
        // });
    }
}