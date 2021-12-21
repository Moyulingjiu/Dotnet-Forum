import * as config from '../utils/config.js'
export const baseUrl = '/api'

export function service({
	url,
	data,
	method
}) {
	return new Promise((resolve, reject) => {
		//uni.app 发起网络请求
		uni.request({
			url: baseUrl + url,
			data: data,
			method: method,
			header: {
				'token': config.getToken(),
				// 'content-type': 'application/x-www-form-urlencoded'
			},
			sslVerify: false, // 进行ssl验证（我们的访问都是http的不应爱进行检验）
			success: (res) => {
				console.log('回调函数成功执行')
				if (res.data.code == 200) {
					//请求成功
					resolve(res.data)
				} else {
					//进行失败回调
					reject(res)
				}
			},
			fail: (errot) => {
				//请求错误 直接进行错误回调
				reject(errot)
			}
		})
	}).catch((e) => {
		console.log(e)
	})
}