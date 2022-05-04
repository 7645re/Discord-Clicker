function minNum (value) {
    if (value > 1) {
        var suffixes = ["", "K", "M", "B", "T"];
        var suffixNum = Math.floor((""+value).length/3);
        var shortValue = parseFloat((suffixNum !== 0 ? (value / Math.pow(1000,suffixNum)) : value).toPrecision(2));
        if (shortValue % 1 !== 0) {
            var shortNum = shortValue.toFixed(1);
        }
        return shortValue+suffixes[suffixNum];
    }
    return value
}

async function buyBuildAsync(id) {
    let result = await buyBuildAsyncRequest(id, lUser.money)
    let buildElement = document.querySelector('[buildId=' + '"' + id + '"' + ']')
    console.log(result)
    if (result.status === "ok") {
        lUser = result["user"]
        localStorage.setItem("user", JSON.stringify(result["user"]))
        // let user = JSON.parse(localStorage.getItem("user"))
        // // user.money = result["money"]
        // // if 
        // user.builds[id].ItemCount+=1
        // localStorage.setItem("user", JSON.stringify(user))
        //
        //
        //
        //
        // // lUser.builds[id].itemCount++
        // buyBuildLS(result["money"], id)
        // let cost = buildElement.getElementsByClassName("build-cost")[0]
        // cost.innerHTML = Math.floor(10 * (1.15**lUser.builds[id].ItemCount))
    }
    console.log(result, lUser)
}   


async function genBuildsCardsAsync() {
    for await (let build of lBuilds) {
        let lvl = lUser.builds[build.id] === undefined ? 0 : lUser.builds[build.id].ItemCount
        builds_list.innerHTML += `
                    <div class="item-card col-sm-12 col-md-4">
                        <div class="build" id="build" buildid="${build.id}" onclick="buyBuildAsync(${build.id})">
                            <div class="build-img" id="${build.id}"><img src="images/buildsImg/svgs/${build.name}.svg"/></div>
                                <div class="build-name" id="${build.id}">${build.name}</div>
                                    <div class="build-cost" id="${build.id}">
                                        <img width="25" src="https://pnggrid.com/wp-content/uploads/2021/05/Black-and-white-Discord-Logo-1024x784.png"/>
                                        ${minNum(build.cost)}
                                    </div>
                                <div class="build-purchased" id="${build.id}"><span>Lvl ${lvl}</span></div>
                            <div class="build-passive" id="${build.id}">Cps +${minNum(build.passiveCoefficient)}</div>
                        </div>
                    </div>`
    }
}