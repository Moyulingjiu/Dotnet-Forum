import * as config from '../utils/config.js'

export function service({
	url,
	data,
	method
}) {
	return new Promise((resolve, reject) => {
		//uni.app 发起网络请求
		uni.request({
			url: config.baseUrl + url,
			data: data,
			method: method,
			sslVerify: false, // 进行ssl验证（我们的访问都是http的不应爱进行检验）
			success: (res) => {
				console.log('请求成功')
				console.log(res)
				console.log(res.data)
				if (res.data.code == 200) {
					//请求成功
					resolve(res.data)
				} else {
					//请求失败 提示用户
					uni.showToast({
						title: '用户名或密码错误',
						icon: "error",
						mask: true,
						duration: 2000
					});
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