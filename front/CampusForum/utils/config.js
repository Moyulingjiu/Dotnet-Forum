// 全局常量配置
export const baseUrl = '/api'

// 检查登陆
export function checkToken() {
	let token = uni.getStorageSync('token')
	if (token == '')
		return false
	return true
}

// 保存token
export function saveToken(token) {
	try {
		uni.setStorageSync('token', token)
		return true
	} catch (e) {
		console.log(e);
		return false
	}
}

// 强制保存token
export function saveTokenFroce(token) {
	let finish = false
	let tryTimes = 0
	const maxTimes = 5 // 最大尝试次数
	while (!finish && tryTimes < maxTimes) {
		finish = saveToken(token)
		tryTimes++
	}
	return finish
}

// 获取token
export function getToken() {
	try {
		return uni.getStorageSync('token')
	} catch (e) {
		console.log(e)
		return null
	}
}

// 保存登陆的用户信息
export function saveUser(user) {
	try {
		uni.setStorageSync('user', user)
		return true
	} catch (e) {
		console.log(e);
		return false
	}
}

// 获取登陆的用户信息
export function getUser() {
	try {
		return uni.getStorageSync('user')
	} catch (e) {
		console.log(e)
		return null
	}
}

// 加密
export function encode(plaintext) {
	return ""
}

// 解密
export function decode(ciphertext) {
	return ""
}

// 获取性别
export function getGender(gender) {
	if (gender == 0)
		return '保密'
	else if (gender == 1)
		return '男'
	return '女'
}

// 检验手机号合法性
export function checkPhone(str) {
	let reMobile = /^1\d{10}$/ // 手机号码
	let reFix = /^0\d{2,3}-?\d{7,8}$/ // 固定座机
	if (reMobile.test(str))
		return true
	if (reFix.test(str))
		return true
	return false
}

// 检验邮箱合法性
export function checkEmail(str) {
	let re = /^(\w-*\.*)+@(\w-?)+(\.\w{2,})+$/
	if (re.test(str))
		return true
	return false
}
