function buyBuildLS(money, buildId) {
    let user = JSON.parse(localStorage.getItem("user"))
    user.money = money
    user.builds[buildId].ItemCount+=1
    localStorage.setItem("user", JSON.stringify(user))
}