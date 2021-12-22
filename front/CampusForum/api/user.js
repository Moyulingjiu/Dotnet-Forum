import * as config from '../utils/config.js'
import * as request from './request.js'
export const modelUrl = `/user`

// 注册
export function register({
	studentId, 
	password, 
	name, 
	college, 
	gender, 
	avater, 
	description, 
	birthday, 
	phone, 
	email
}) {
	let url = modelUrl + `/register`
	let data = {
		studentId: studentId,
		password: password,
		name: name,
		college: college,
		gender: gender,
		avater: avater,
		description: description,
		birthday: birthday,
		phone: phone,
		email: email
	}
	return request.service({
		url: url,
		data: data,
		method: 'POST'
	})
}

// 登陆
export function login(studentId, password) {
	let url = modelUrl + `/login`
	let data = {
		studentId: studentId,
		password: password
	}
	return request.service({
		url: url,
		data: data,
		method: 'GET'
	})
}

// 登出
export function logout() {
	let url = modelUrl + `/logout`
	return request.service({
		url: url,
		data: {},
		method: 'GET'
	})
}

// 修改用户数据
export function updateById(id,{
		name: name,
		college: college,
		gender: gender,
		description: description,
		birthday: birthday,
		phone: phone,
		email: email}) 
	{
	let url = modelUrl + `/update/${id}`
	let data={
		name: name,
		college: college,
		gender: gender,
		description: description,
		birthday: birthday,
		phone: phone,
		email: email}
	// 请求
	return request.service({
		url: url,
		data: data,
		method: 'POST'
	})
}

// 删除用户
export function deleteById(id) {
	let url = modelUrl + `/delete/${id}`
	return request.service({
		url: url,
		data: {},
		method: 'POST'
	})
}


// 根据id获取用户数据
export function select(id=0) {
	let url = modelUrl + `/select/${id}`
	console.log(url)
	return request.service({
		url: url,
		data: {},
		method: 'GET'
	})
}

// 查询所有的用户
export function selectAll(page=0, pageSize=10) {
	let url = modelUrl + `/selectAll`
	let data = {
		page: page,
		pageSize: pageSize
	}
	return request.service({
		url: url,
		data: data,
		method: 'GET'
	})
}

// 模糊查询
export function selectCondition(page=0, pageSize=10, id, studentId, name, college, gender, beginDate, endDate) {
	let url = modelUrl + `/selectCondition`
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
	let data = {
		page: page,
		pageSize: pageSize
	}
	return request.service({
		url: url,
		data: data,
		method: 'GET'
	})
}

// 获取关注列表
export function followings(page=0, pageSize=10) {
	let url = modelUrl + `/followings`
	let data = {
		page: page,
		pageSize: pageSize
	}
	return request.service({
		url: url,
		data: data,
		method: 'GET'
	})
}

// 关注某人
export function follow(id) {
	let url = modelUrl + `/follow/${id}`
	return request.service({
		url: url,
		data: {},
		method: 'POST'
	})
}

// 取关某人
export function unfollow(id) {
	let url = modelUrl + `/unfollow/${id}`
	return request.service({
		url: url,
		data: {},
		method: 'POST'
	})
}