<template>
	<view>
		<view class="insert_box" @click="newState()">
			<image src="../../static/insert.png" mode="aspectFill"></image>
			<view>
				<text>新增状态</text>
			</view>
		</view>
		<view class="state_box" v-for="(item,index) in stateList">
			<view class="state_title">
				<text>{{ item.title }}</text>
			</view>
			<view class="state_text">
				<text space="emsp">{{ item.text }}</text>
			</view>
			<view class="state_bottom">
				<text class="state_date">{{ item.gmtCreate }}</text>
				<view class="state_icon">
					<image src="../../static/state.png" mode="aspectFill" @click="updateState(index)"></image>
					<image src="../../static/cancel.png" mode="aspectFill" @click="deleteState(index)"></image>
				</view>
			</view>
		</view>

		<view class="bottom_tips">
			<text>{{ (page >= total - 1) ? bottomTipsNoMore : bottomTips }}</text>
		</view>
		
		<view class="newstate" @tap="newState()">
			<view class="circle"></view>
			<view class="plus">+</view>
		</view>

		<uni-popup ref="popup_success" type="message">
			<uni-popup-message type="success" message="删除成功" :duration="2000"></uni-popup-message>
		</uni-popup>
		<uni-popup ref="popup_success_refresh" type="message">
			<uni-popup-message type="success" message="刷新成功" :duration="2000"></uni-popup-message>
		</uni-popup>
	</view>
</template>

<script>
	import * as userApi from "../../api/user.js"
	import * as stateApi from "../../api/state.js"
	import * as config from "../../utils/config.js"

	export default {
		data() {
			return {
				bottomTips: '- 上拉加载更多 -', // 底部提示
				bottomTipsNoMore: '- 到底了 -', // 底部提示
				isRefresh: true, // 是否需要刷新
				page: 0, // 当前页码
				total: 1, // 最后一页
				stateList: [{
					id: 1,
					title: '测试状态标题1',
					userId: 1, // 用户id
					userName: '墨羽翎玖', // 用户名
					userAvater: '../../static/avater.jpg', // 头像
					text: '测试的正文1',
					share: true,
					gmtCreate: '2021年7月20日 20:00',
				}]
			}
		},
		onLoad() {
			this.isRefresh = true
		},
		onShow() {
			this.refresh()
		},
		onPullDownRefresh() {
			this.isRefresh = true
			this.refresh()
			uni.stopPullDownRefresh();
			this.$refs.popup_success_refresh.open('top')
		},
		methods: {
			refresh() {
				let temp = uni.getStorageSync('refreshState')
				if (temp === true) {
					uni.setStorageSync('refreshState', false)
					this.isRefresh = true
				}
				if (config.checkToken()) {
					if (this.isRefresh) {
						this.isRefresh = false
						this.stateList = []
						this.page = 0
						this.loadData()
					}
				} else {
					uni.redirectTo({
						url: '../login/login'
					})
				}
			},
			loadData(page = this.page) {
				stateApi.selectAll(page).then(data => {
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
						this.total = data.data.total
						for (let key in data.data.items) {
							if (key != 'length') {
								let stateItem = {
									id: data.data.items[key].id,
									title: data.data.items[key].title,
									userId: data.data.items[key].user_id,
									userName: data.data.items[key].user_name,
									userAvater: data.data.items[key].user_avater, // 头像
									text: data.data.items[key].text,
									share: data.data.items[key].share_state,
									gmtCreate: data.data.items[key].gmt_create
								}
								this.stateList.push(stateItem)
							}
						}
					}
				})
			},
			newState() {
				uni.navigateTo({
					url: '/pages/editStatus/editStatus'
				});
			},
			deleteState(index) {
				let stateId = this.stateList[index].id
				new Promise((resolve, reject) => {
					uni.showModal({
						title: '确定删除此状态吗?',
						content: ' ',
						success: function(res) {
							resolve(res)
						}
					})
				}).then(data => {
					if (data.confirm) {
						return stateApi.deleteState(stateId)
					}
				}).then(data => {
					if (typeof data === "undefined") {
						
					} else if (data.data) {
						this.$refs.popup_success.open('top')
						this.isRefresh = true
						this.stateList.splice(index, 1)
					} else {
						uni.showToast({
							title: data.msg,
							icon: "error",
							mask: true,
							duration: 2000
						})
					}
				})
			},
			updateState(index) {
				let stateId = this.stateList[index].id
				uni.navigateTo({
				    url: `/pages/editStatus/updateStatus?id=${stateId}`
				})
			}
		}
	}
</script>

<style>
	.insert_box {
		width: 90%;
		margin-left: 5%;
		margin-right: 5%;
		margin-top: 40rpx;
		padding-left: 10rpx;
		padding-top: 10rpx;
		padding-bottom: 10rpx;
		border-radius: 10rpx;
		background-color: #F0F0F0;
		box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
		text-align: center;
	}

	.insert_box image {
		width: 50rpx;
		height: 50rpx;
	}

	.insert_box view {
		width: 100%;
		text-align: center;
	}

	.state_box {
		width: 90%;
		margin-left: 5%;
		margin-right: 5%;
		margin-top: 40rpx;
		padding-left: 10rpx;
		padding-top: 10rpx;
		padding-bottom: 10rpx;
		border-radius: 10rpx;
		background-color: #F5F5F5;
		box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
	}

	.state_box .state_title {
		font-size: 45rpx;
		font-weight: bold;
		/* text-shadow: 1px 1px 4px #000000; */
	}

	.state_box .state_author {
		display: flex;
		margin-top: 10rpx;
		margin-left: 10rpx;
	}

	.state_box .state_author image {
		height: 45rpx;
		width: 45rpx;
		border-radius: 50%;
	}

	.state_box .state_author text {
		margin-top: 3rpx;
		margin-left: 10rpx;
		font-weight: bold;
		font-size: 30rpx;
		color: #6F6F6F;
	}

	.state_box .state_text {
		padding-left: 10rpx;
		padding-right: 10rpx;
	}

	.state_box .state_text text {
		margin-top: 20rpx;
		font-size: 35rpx;
		display: -webkit-box;
		/*弹性伸缩盒子模型显示*/
		-webkit-box-orient: vertical;
		/*排列方式*/
		-webkit-line-clamp: 8;
		/*显示文本行数*/
		overflow: hidden;
		/*溢出隐藏*/
	}

	.state_box .state_bottom {
		padding-top: 20rpx;
		position: relative;
	}

	.state_box .state_bottom .state_date {
		color: #A8A8A8;
		font-size: 30rpx;
		width: 80%;
		position: relative;
		display: inline-block;
	}

	.state_box .state_bottom .state_icon {
		margin-bottom: 10rpx;
		margin-right: 10rpx;
		height: 30rpx;
		position: absolute;
		bottom: 0;
		right: 0;
	}

	.state_box .state_bottom .state_icon image {
		margin-left: 15rpx;
		width: 30rpx;
		height: 30rpx;
	}

	.bottom_tips {
		margin-top: 30rpx;
		text-align: center;
		height: 200rpx;
	}

	.bottom_tips text {
		color: #A8A8A8;
		bottom: 0;
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
