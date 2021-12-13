// 全局常量配置
export const baseUrl = 'localhost:8080'

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

// 加密
export function encode(plaintext) {
	return ""
}

// 解密
export function decode(ciphertext) {
	return ""
}