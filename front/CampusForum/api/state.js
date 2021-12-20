import * as config from '../utils/config.js'
export const modelUrl = config.baseUrl +  `/state`

// 新建状态
export function insert(title, text, shareSate=false) {
	let url = modelUrl + `/insert`
	let token = config.getToken()
	// 请求
	let stateId = 1
	return stateId
}

// 修改状态
export function update(stateId, data) {
	let url = modelUrl + `/update/${stateId}`
	let token = config.getToken()
	// 请求
	return true
}

// 删除状态
export function delete(stateId) {
	let url = modelUrl + `/delete/${stateId}`
	let token = config.getToken()
	// 请求
	return true
}

// 通过id查询状态
export function select(stateId) {
	let url = modelUrl + `/select/${stateId}`
	let token = config.getToken()
	
	return {
		id: 1,
		title: '测试状态',
		text: '正文',
		shareSate: true,
		like: false,
		likeNumber: 10,
		userId: 1,
		userName: '作者名',
		userAvater: '用户头像'
	}
}

// 查询所有状态
export function selectAll(page=0, pageSize=10) {
	let url = modelUrl + `/selectAll`
	let token = config.getToken()
	
	return {
		total: 1,
		item: []
	}
}

// 条件查询状态
export function selectCondition(page=0, pageSize=10, userId, userName, title) {
	let url = modelUrl + `/selectCondition`
	let token = config.getToken()
	
	return {
		total: 1,
		item: []
	}
}

// 主页推荐
export function recommend(page=0, pageSize=10) {
	let url = modelUrl + `/recommend`
	let token = config.getToken()
	
	return {
		total: 1,
		item: []
	}
}

// 点赞状态
export function like(stateId) {
	let url = modelUrl + `/like/${stateId}`
	let token = config.getToken()
	return true
}

// 取消点赞状态
export function unlike(stateId) {
	let url = modelUrl + `/unlike/${stateId}`
	let token = config.getToken()
	return true
}