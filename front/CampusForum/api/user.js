import * as config from '../utils/config.js'

// 根据id获取用户数据
export function getUserById(id) {
	let url = config.baseUrl + `/getUser/${id}`
	let data = {}
	// uni.request({
	// 	url: url,
	// 	data: data,
	// 	method: 'GET',
	// 	timeout: 50000,
	// 	success() {
	// 		// 成功回调函数
	// 	},
	// 	fail() {
	// 		// 失败回调函数
	// 	}
	// })
	return {
		name: "用户名"
	}
}

// 根据token获取用户数据
export function getUserByToken(token) {
	let url = config.baseUrl + `/getUserByToken`
	let data = {
		token: token
	}
	return null
}


// 登陆
export function login(id, password) {
	let url = config.baseUrl + `/login/${id}`
	let data = {
		password: password
	}
	return null
}

// 登出
export function logout(id) {
	return null
}

// 注册
export function register(data) {
	return null
}

// 通过用户注册
export function allowRegisterById(id) {
	return null
}

// 修改用户数据
export function updateUserById(data) {
	return null
}