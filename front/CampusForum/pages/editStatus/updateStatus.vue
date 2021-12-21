<template>
	<view>
		<view class="state_title">
			<input placeholder="在此输入标题" maxlength="20" type="text" v-model="state.title" />
		</view>
		<view class="state_text">
			<textarea maxlength="2000" auto-height placeholder="在此输入正文" v-model="state.text" />
		</view>
		<button class="bottom" @click="updateState()">修改状态</button>
		<view class="state_share">
			<text>是否分享该状态</text>
			<switch :checked="getShare()" @change="switchShare" />
		</view>
		<view class="state_create">
			<text>创建时间：{{ state.gmtCreate }}</text>
		</view>
		<uni-popup ref="popup_success" type="message">
			<uni-popup-message type="success" message="修改成功" :duration="1500"></uni-popup-message>
		</uni-popup>
		<uni-popup ref="popup_error" type="message">
			<uni-popup-message type="error" message="标题和正文不能为空" :duration="1500"></uni-popup-message>
		</uni-popup>
	</view>
</template>

<script>
	import * as stateApi from '../../api/state.js'
	import * as config from '../../utils/config.js'

	export default {
		data() {
			return {
				stateId: 0,
				state: {
					title: '',
					text: '',
					shareState: 1, // 对于该点使用数字来表示状态
					gmtCreate: ''
				}
			}
		},
		onLoad(option) {
			this.stateId = option.id
		},
		onShow() {
			this.refresh()
		},
		methods: {
			refresh() {
				if (!config.checkToken()) {
					uni.redirectTo({
						url: '../login/login'
					})
				} else if (this.stateId == 0 || typeof this.stateId === "undefined") {
					uni.reLaunch({
						url: '../state/state'
					})
				} else {
					stateApi.select(this.stateId).then(data => {
						if (typeof data === "undefined") {
							uni.showToast({
								title: "服务器错误",
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
							this.state.title = data.data.title
							this.state.text = data.data.text
							this.state.shareState = data.data.share_state
							this.state.gmtCreate = data.data.gmt_create
						}
					})
				}
			},
			updateState() {
				if (this.state.title.length == 0 || this.state.text.length == 0) {
					this.$refs.popup_error.open('top')
				} else {
					stateApi.update(this.stateId, this.state.title, this.state.text, this.state.shareState).then(data => {
						if (typeof data === "undefined") {
							uni.showToast({
								title: '服务器错误',
								icon: "error",
								mask: true,
								duration: 2000
							})
						} else {
							if (data.code == 200) {
								this.$refs.popup_success.open('top')
								uni.setStorageSync('refreshState', true)
							} else {
								uni.showToast({
									title: data.msg,
									icon: "error",
									mask: true,
									duration: 2000
								})
							}
						}
					})
				}
			},
			switchShare() {
				if (this.state.isShare == 0) {
					this.state.isShare = 1
				} else {
					this.state.isShare = 0
				}
			},
			getShare() {
				if (this.state.isShare == 0)
					return false
				return true
			}
		}
	}
</script>

<style>
	.state_title {
		margin: 15rpx 5%;
		width: 90%;
		border-bottom: solid 3rpx #808080;
	}

	.state_title input {
		font-size: 50rpx;
		font-weight: bold;
	}

	.state_title:hover {
		border-bottom: solid 5rpx #e96d29;
	}

	.state_text {
		margin: 30rpx 5%;
		width: 90%;
	}

	.state_text textarea {
		width: 100%;
		font-size: 35rpx;
	}

	.state_text textarea:hover {
		/* border: solid 3rpx #f1707d; */
	}

	.state_share {
		margin: 50rpx 5% 0rpx 5%;
		width: 90%;
		display: flex;
	}

	.state_share text {
		margin-top: 5rpx;
		font-size: 45rpx;
	}

	.state_share switch {
		right: 0;
		position: absolute;
	}

	.state_create {
		margin: 30rpx 5%;
		width: 90%;
		display: flex;
	}

	.state_create text {
		margin-top: 5rpx;
		font-size: 45rpx;
		color: #808080;
	}

	.bottom {
		width: 90%;
		margin: 30rpx 5%;
		box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
		background-image: linear-gradient(120deg, #a1c4fd 0%, #c2e9fb 100%);
	}
</style>
