import * as config from '../utils/config.js'
import request from './request.js'
export const modelUrl = `/user`

// 注册
export function register(studentId, password, name, college, gender, avater, description, birthday, phone, email) {
	let url = modelUrl + `/register`
	let data = {
		
	}
	request({
		url: url,
		data: data,
		method: 'POST'
	})
	return null
}

// 登陆
export function login(studentId, password) {
	let url = modelUrl + `/login`
	let data = {
		studentId: studentId,
		password: password
	}
	request({
		url: url,
		data: data,
		method: 'GET'
	})
	
	let token = '123' // 模拟获取token值
	return config.saveToken(token)
}

// 登出
export function logout() {
	let url = modelUrl + `/logout`
	let token = config.getToken()
	config.saveTokenFroce('') // 删除token的值
	return true
}

// 修改用户数据
export function updateById(id) {
	let url = modelUrl + `/update/${id}`
	let token = config.getToken()
	// 请求
	return true
}

// 删除用户
export function deleteById(id) {
	let url = modelUrl + `/delete/${id}`
	let token = config.getToken()
	// 请求
	return true
}


// 根据id获取用户数据
export function select(id) {
	let url = modelUrl + `/select/${id}`
	let token = config.getToken()
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
		id: 1,
		studentId: 22920192200000,
		name: '用户名',
		college: '信息学院',
		gender: 1,
		avater: '',
		description: '个性签名',
		birthday: '2021-12-20',
		phone: '',
		email: '',
		follwer: 0,
		follwing: 0
	}
}

// 查询所有的用户
export function selectAll(page=0, pageSize=10) {
	let url = modelUrl + `/selectAll`
	let token = config.getToken()
	let data = {
		token: token,
		page: page,
		pageSize: pageSize
	}
	return {
		total: 1,
		item: []
	}
}

// 模糊查询
export function selectCondition(page=0, pageSize=10, id, studentId, name, college, gender, beginDate, endDate) {
	let url = modelUrl + `/selectCondition`
	let token = config.getToken()
	let data = {
		token: token,
		id: id,
		studentId: studentId,
		name: name,
		college: college,
		gender: gender,
		beginDate: beginDate,
		endDate: endDate,
		page: page,
		pageSize: pageSize
	}
	// 模拟请求
	return {
		total: 1,
		item: []
	}
}

// 获取粉丝列表
export function followers(page=0, pageSize=10) {
	let url = modelUrl + `/followers`
	let token = config.getToken()
	// 模拟请求
	return {
		total: 1,
		item: []
	}
}

// 获取关注列表
export function followings(page=0, pageSize=10) {
	let url = modelUrl + `/followings`
	let token = config.getToken()
	// 模拟请求
	return {
		total: 1,
		item: []
	}
}

// 关注某人
export function follow(id) {
	let url = modelUrl + `/follow/${id}`
	let token = config.getToken()
	// 模拟请求
	return true
}

// 取关某人
export function unfollow(id) {
	let url = modelUrl + `/unfollow/${id}`
	let token = config.getToken()
	// 模拟请求
	return true
}