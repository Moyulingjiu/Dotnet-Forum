<template>
	<view>
		<button type="primary" @click="chooseAndUploadPic()">请选择图片</button>

		<view class="container" :style="{'--picWidth':this.picWidth}">
			<view v-for="(item,index) in this.picture.item">
				<template>
					<image :src="item.url" mode="aspectFill" class="text" @click="clickPic(index, item.url)"></image>
				</template>
			</view>
		</view>

		<uni-popup ref="popup_success" type="message">
			<uni-popup-message type="success" message="删除成功" :duration="3000"></uni-popup-message>
		</uni-popup>
	</view>

</template>


<script>
	import * as config from '../../utils/config.js'
	import * as pictureApi from '../../api/picture.js'
	import * as albumApi from '../../api/album.js'
	export default {
		data() {
			return {
				percent: 0,
				delete: 0,
				albumId: 0,
				albumName: "",
				albumDescription: "",
				windowWidth: 0,
				picWidthNum: 0,
				picWidth: "",
				picRowNum: 0,
				picture: {
					total: 3,
					page: 0,
					item: [{
							"Id": "1",
							"Name": "asd",
							"url": "https://ns-strategy.cdn.bcebos.com/ns-strategy/upload/fc_big_pic/part-00563-2872.jpg"
						},
						{
							"Id": "2",
							"Name": "asdsad",
							"url": "https://ns-strategy.cdn.bcebos.com/ns-strategy/upload/fc_big_pic/part-00563-2872.jpg"
						},
						{
							"Id": "3",
							"Name": "asdddd",
							"url": "https://ns-strategy.cdn.bcebos.com/ns-strategy/upload/fc_big_pic/part-00563-2872.jpg"
						}
					]
				},
				loadLock: false
			}
		},
		onNavigationBarButtonTap: function(e) {
			if (this.delete == 0) {
				this.delete = 1
				uni.showToast({
					title: '进入删除模式，点击相片删除，再次点击删除按钮退出删除模式',
					icon: "error",
					mask: true,
					duration: 2000
				})
			} else {
				this.delete = 0
				uni.showToast({
					title: '退出删除模式',
					icon: "error",
					mask: true,
					duration: 2000
				})
			}

		},
		onLoad(options) {
			this.albumId = options.id
			if (typeof this.albumId == 'undefined' || this.albumId == 0) {
				uni.switchTab({
					url: '../album/album'
				})
			}

			let that = this
			uni.getSystemInfo({
				success: function(res) {
					that.windowWidth = res.windowWidth;
					that.picRowNum = Math.ceil(Math.max(3, that.windowWidth / 250));
					that.picWidthNum = (that.windowWidth - 1) / that.picRowNum;
					that.picWidth = that.picWidthNum + "px";
				}
			})
		},
		onShow() {
			this.refresh()
		},
		onReachBottom() {
			if (!this.loadLock) { // 加锁
				this.loadLock = true
				if (this.picture.page < this.picture.total - 1) {
					this.picture.page++
					this.loadData()
				}
				this.loadLock = false
			}
		},
		methods: {
			refresh() {
				if (config.checkToken()) {
					this.picture.page = 0
					this.picture.item = []
					this.loadData()
				} else {
					uni.redirectTo({
						url: '../login/login'
					})
				}
			},
			loadData(page = this.picture.page) {
				pictureApi.getPictureByAlbumId(this.albumId, page).then(data => {
					if (typeof data === "undefined") {
						uni.showToast({
							title: '服务器错误',
							icon: "error",
							mask: true,
							duration: 2000
						})
					} else if (data.code != 200) {
						uni.showToast({
							title: data.msg,
							icon: "error",
							mask: true,
							duration: 2000
						})
					} else {
						this.picture.total = data.data.total
						for (let key in data.data.item) {
							let temp = {
								id: data.data.item[key].id,
								name: data.data.item[key].name,
								url: data.data.item[key].url,
							}
							temp.url = "/api" + String(temp.url).replace(/\\/g,"/")
							this.picture.item.push(temp)
						}
						if (data.data.item.length > 0) {
							albumApi.updateAlbum(this.albumId, String(data.data.item[0].url).replace(/\\/g,"/"))
						}
					}
				})
			},
			clickPic(index, url='') {
				let that = this
				if (this.delete == 0) {
					console.log(url)
					window.location.href = String(url).replace(/\/api/g, "http://localhost:20673")
					uni.navigateTo({
						url: String(url).replace(/\/api/g, "http://localhost:20673")
					});
				} else {
					uni.showModal({
						title: '确定要删除此相片吗?',
						content: ' ',
						success: function(res) {
							if (res.confirm) {
								pictureApi.deletePicture(that.picture.item[index].id).then(data => {
									if (typeof data === "undefined") {
										uni.showToast({
											title: '服务器错误',
											icon: "error",
											mask: true,
											duration: 2000
										})
									} else if (data.code == 200) {
										that.$refs.popup_success.open('top')
										that.refresh();
									} else {
										uni.showToast({
											title: '删除失败:' + data.msg,
											icon: "error",
											mask: true,
											duration: 2000
										})
									}
								})
							}
						}
					});
				}
			},
			chooseAndUploadPic() {
				new Promise((resolve, reject) => {
					uni.chooseImage({
						count: 1,
						sizeType: ['copressed'],
						success(res) {
							resolve(res)
						}
					})
				}).then(data => {
					if (typeof data != 'undefined') {
						return new Promise((resolve, reject) => {
							//因为有一张图片， 输出下标[0]， 直接输出地址
							let imgFiles = data.tempFilePaths[0]
							console.log(this.albumId)
							// 上传图片
							// 做成一个上传对象
							uni.uploadFile({
								// 需要上传的地址
								url: `http://localhost:20673/picture/insert/${this.albumId}`,
								header: {
									'token': config.getToken(),
								},
								// filePath  需要上传的文件
								filePath: imgFiles,
								name: 'file',
								success(res) {
									resolve(res)
								},
								fail(err) {
									console.log(err)
								}
							})
						})
					}
				}).then(data => {
					if (typeof data != 'undefined') {
						this.refresh()
						console.log(data)
					}
				})

			},

		}
	}
</script>

<style>
	.container {
		/* 定义flex容器 */
		display: flex;
		/*设置容器内部容器的排列方向*/
		flex-direction: row;
		flex-wrap: wrap;
	}

	.green {
		background-color: green;
	}

	.blue {
		background-color: blue;
	}

	.red {
		background-color: red;
	}

	.black {
		background-color: black;
	}

	.orange {
		background-color: orange;
	}

	.text {
		font-size: 20upx;
		width: var(--picWidth);
		height: var(--picWidth);
	}

	.newstate {
		position: fixed;
		right: 20rpx;
		bottom: 200rpx;
	}

	.circle {
		width: 50px;
		height: 50px;
		background-color: #2979FF;
		border-radius: 50%;
	}

	.plus {
		position: absolute;
		top: 3px;
		right: 14px;
		font-size: 30px;
	}
</style>
