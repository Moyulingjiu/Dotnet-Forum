<template>
	<view>
		<view class="person_card">
			<view class="flex_box">
				<image class="person_avater" :src="user.avater" mode="aspectFill"></image>
				<view class="person_information">
					<text class="person_name">{{ user.name }}</text>
					<view class="follow_container">
						<view :class="user.follow?'follow':'unfollow'" @click="changeFollow()">
							{{ user.follow?'已关注':'关注' }}
						</view>
					</view>
					<view class="person_description">
						<text>{{ user.description }}</text>
					</view>
					<text class="person_follow">关注：</text>
					<text class="person_follow">{{ user.following }}</text>
					<text class="person_follow inner_split">粉丝：</text>
					<text class="person_follow">{{ user.follower }}</text>
				</view>
			</view>
		</view>
		
		<view class="person_card show_detail" v-if="!showDetail" @click="changeShowDetail()">
			点击展示详细个人信息
		</view>
		
		<view class="person_card" v-if="showDetail">
			<view class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>注册时间</text>
					</view>
					<view class="information_text">
						<text>{{ user.gmtCreate }}</text>
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>学号</text>
					</view>
					<view class="information_text">
						<text>{{ user.studentId }}</text>
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>学院</text>
					</view>
					<view class="information_text">
						<text>{{ user.collegeName }}</text>
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>性别</text>
					</view>
					<view class="information_text">
						<text>{{ getGender() }}</text>
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>电话</text>
					</view>
					<view class="information_text">
						<text>{{ user.phone }}</text>
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>邮箱</text>
					</view>
					<view class="information_text">
						<text>{{ user.email }}</text>
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>生日</text>
					</view>
					<view class="information_text">
						<text>{{ user.birthday }}</text>
					</view>
				</view>
			</view>
		</view>

		<view class="person_card"
			v-if="showDetail&&user.primarySchool+user.juniorHighSchool+user.highSchool+user.university!=''">
			<view v-if="user.primarySchool!=''" class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>小学</text>
					</view>
					<view class="information_text">
						<text>{{ user.primarySchool }}</text>
					</view>
				</view>
			</view>
			<view v-if="user.juniorHighSchool!=''" class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>初中</text>
					</view>
					<view class="information_text">
						<text>{{ user.juniorHighSchool }}</text>
					</view>
				</view>
			</view>
			<view v-if="user.highSchool!=''" class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>高中</text>
					</view>
					<view class="information_text">
						<text>{{ user.highSchool }}</text>
					</view>
				</view>
			</view>
			<view v-if="user.university!=''" class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>大学</text>
					</view>
					<view class="information_text">
						<text>{{ user.university }}</text>
					</view>
				</view>
			</view>
		</view>

		<view class="person_card"
			v-if="showDetail&&user.hobby.other+user.hobby.music+user.hobby.book+user.hobby.movie+user.hobby.game+user.hobby.sport!=''">
			<view v-if="user.hobby.other!=''" class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>爱好</text>
					</view>
					<view class="information_text">
						<text>{{ user.hobby.other }}</text>
					</view>
				</view>
			</view>
			<view v-if="user.hobby.music!=''" class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>喜欢音乐</text>
					</view>
					<view class="information_text">
						<text>{{ user.hobby.music }}</text>
					</view>
				</view>
			</view>
			<view v-if="user.hobby.book!=''" class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>喜欢的书</text>
					</view>
					<view class="information_text">
						<text>{{ user.hobby.book }}</text>
					</view>
				</view>
			</view>
			<view v-if="user.hobby.movie!=''" class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>喜欢的电影</text>
					</view>
					<view class="information_text">
						<text>{{ user.hobby.movie }}</text>
					</view>
				</view>
			</view>
			<view v-if="user.hobby.game!=''" class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>喜欢的游戏</text>
					</view>
					<view class="information_text">
						<text>{{ user.hobby.game }}</text>
					</view>
				</view>
			</view>
			<view v-if="user.hobby.sport!=''" class="information_card flex_box">
				<view class="information_container">
					<view class="information_title">
						<text>喜欢的运动</text>
					</view>
					<view class="information_text">
						<text>{{ user.hobby.sport }}</text>
					</view>
				</view>
			</view>
		</view>

		<view class="state_box" v-for="(item,index) in stateList">
			<view class="state_title" @click="stateDetail(index)">
				<text>{{ item.title }}</text>
			</view>
			<view class="state_author" @click="userdetail(index)">
				<image :src="item.userAvater"></image>
				<text>{{ item.userName }}</text>
			</view>
			<view class="state_text" @click="stateDetail(index)">
				<text space="emsp">{{ item.text }}</text>
			</view>
			<view class="state_bottom">
				<text class="state_date">{{ item.gmtCreate }}</text>
				<view class="state_icon">
					<image src="../../static/comment.png" mode="aspectFill" @click="stateDetail(index)"></image>
					<image :src="item.like?likeIcon:unlikeIcon" @click="like(index)"></image>
				</view>
			</view>
		</view>

		<view class="bottom_tips">
			<text>{{ (page >= total - 1) ? bottomTipsNoMore : bottomTips }}</text>
		</view>

		<uni-popup ref="popup_success" type="message">
			<uni-popup-message type="success" message="登出成功" :duration="3000"></uni-popup-message>
		</uni-popup>
	</view>
</template>

<script>
	import * as config from "../../utils/config.js"
	import * as userApi from "../../api/user.js"
	import * as stateApi from "../../api/state.js"

	export default {
		data() {
			return {
				showDetail: false,
				likeIcon: '../../static/like_active.png', // 点赞图标
				unlikeIcon: '../../static/like.png', // 未点赞图标
				bottomTips: '- 上拉加载更多 -', // 底部提示
				bottomTipsNoMore: '- 到底了 -', // 底部提示
				id: 0,
				user: {
					studentId: 22920191234,
					name: '墨羽翎玖',
					college: '信息学院',
					departmet: '',
					gender: 1,
					avater: '../../static/avater.jpg',
					description: '来时山有雪，归时雪满山。',
					birthday: '2021-12-15',
					phone: '12312341234',
					email: '123456@qq.com',
					follow: false,
					following: 100,
					follower: 100,
					primarySchool: '',
					juniorHighSchool: '',
					highSchool: '',
					university: '',
					gmtCreate: '',
					admin: false,
					hobby: {
						other: '',
						music: '',
						book: '',
						movie: '',
						game: '',
						comic: '',
						sport: ''
					}
				},
				page: 0,
				total: 1,
				stateList: [{
					id: 1,
					title: '测试状态标题1',
					userId: 1, // 用户id
					userName: '墨羽翎玖', // 用户名
					userAvater: '../../static/avater.jpg', // 头像
					text: '测试的正文1',
					share: true,
					gmtCreate: '2021年7月20日 20:00',
					like: true
				}],
				loadLock: false // 上拉加载的锁
			}
		},
		onLoad(options) {
			this.id = options.id
		},
		onShow() {
			this.refresh()
		},
		onReachBottom() {
			if (!this.loadLock) { // 加锁
				this.loadLock = true
				if (this.page < this.total - 1) {
					this.page++
					this.loadData()
				}
				this.loadLock = false
			}
		},
		methods: {
			refresh() {
				if (!config.checkToken()) {
					uni.redirectTo({
						url: '../login/login'
					})
				} else if (this.id == 0 || typeof this.id === "undefined") {
					uni.switchTab({
						url: '../index/index'
					})
				} else {
					userApi.select(this.id).then(data => {
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
							this.user.id = data.data.id
							this.user.studentId = data.data.studentId
							this.user.name = data.data.name
							this.user.college = data.data.college
							this.user.departmet = data.data.department
							this.user.gender = data.data.gender
							// this.user.avater = data.data.avater
							this.user.avater = "/api" + String(data.data.avater).replace(/\\/g, "/")
							this.user.description = data.data.description
							this.user.phone = data.data.phone
							this.user.email = data.data.email

							this.user.following = data.data.following
							this.user.follower = data.data.follower
							this.user.follow = data.data.follow

							this.user.gmtCreate = data.data.gmt_create
							this.user.gmtModified = data.data.gmt_modified

							this.user.admin = data.data.admin

							this.user.primarySchool = data.data.primarySchool == null ? '' : data.data
								.primarySchool
							this.user.juniorHighSchool = data.data.juniorHighSchool == null ? '' : data.data
								.juniorHighSchool
							this.user.highSchool = data.data.highSchool == null ? '' : data.data.highSchool
							this.user.university = data.data.university == null ? '' : data.data.university
						
							this.user.hobby.other = data.data.hobby.other
							this.user.hobby.music = data.data.hobby.music
							this.user.hobby.book = data.data.hobby.book
							this.user.hobby.movie = data.data.hobby.movie
							this.user.hobby.game = data.data.hobby.game
							this.user.hobby.comic = data.data.hobby.comic
							this.user.hobby.sport = data.data.hobby.sport
						}
					})
					this.page = 0
					this.total = 1
					this.stateList = []
					this.loadData()
				}
			},
			getGender() {
				return config.getGender(this.user.gender)
			},
			changeFollow() {
				if (this.user.follow) {
					userApi.unfollow(this.id).then(data => {
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
							if (data.data) {
								this.user.follow = false
								this.refresh()
							}
						}
					})
				} else {
					userApi.follow(this.id).then(data => {
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
							if (data.data) {
								this.user.follow = true
								this.refresh()
							}
						}
					})
				}
			},
			loadData(page = this.page) {
				stateApi.selectAll(page, 10, this.id).then(data => {
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
							let stateItem = {
								id: data.data.items[key].id,
								title: data.data.items[key].title,
								userId: data.data.items[key].userId,
								userName: data.data.items[key].userName,
								userAvater: data.data.items[key].userAvater, // 头像
								text: data.data.items[key].text,
								share: data.data.items[key].share_state,
								gmtCreate: data.data.items[key].gmt_create,
								like: data.data.items[key].like
							}
							stateItem.userAvater = "/api" + String(stateItem.userAvater).replace(/\\/g, "/")
							this.stateList.push(stateItem)
						}
					}
				})
			},
			changeShowDetail() {
				this.showDetail = !this.showDetail
			},
			stateDetail(index) {
				let id = this.stateList[index].id
				uni.navigateTo({
					url: `/pages/stateDetail/stateDetail?id=${id}`
				})
			},
			like(index) {
				let id = this.stateList[index].id
				if (this.stateList[index].like) {
					stateApi.unlike(id).then(data => {
						if (typeof data == "undefined") {
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
							this.stateList[index].like = false
							this.$forceUpdate()
						}
					})
				} else {
					stateApi.like(id).then(data => {
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
							this.stateList[index].like = true
							this.$forceUpdate()
						}
					})
				}
			}
		}
	}
</script>

<style>
	.flex_box {
		display: flex;
	}

	.inner_split {
		margin-left: 20rpx;
	}

	.person_card {
		width: 90%;
		margin-left: 5%;
		margin-right: 5%;
		margin-top: 40rpx;
		/* height: 160rpx; */
		background-color: #F0F0F0;
		padding-left: 10rpx;
		padding-top: 10rpx;
		padding-bottom: 10rpx;
		border-radius: 10rpx;
		box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
	}
	
	.show_detail {
		text-align: center;
		font-size: 35rpx;
	}

	.person_avater {
		width: 150rpx;
		height: 150rpx;
		border-radius: 50%;
		margin-right: 20rpx;
	}

	.person_information {
		/* margin-top: 10rpx; */
		margin-left: 10rpx;
	}

	.person_name {
		font-size: 50rpx;
		font-weight: bold;
		width: 400rpx;
		display: -webkit-box;
		/*弹性伸缩盒子模型显示*/
		-webkit-box-orient: vertical;
		/*排列方式*/
		-webkit-line-clamp: 1;
		/*显示文本行数*/
		overflow: hidden;
		/*溢出隐藏*/
	}
	
	.follow_container {
		position: absolute;
		top: -20rpx;
		right: 10rpx;
	}

	.person_follow {
		font-size: 30rpx;
		color: #5F5F5F;
	}

	.person_description {
		/* margin-top: 10rpx; */
		/* margin-left: 20rpx; */
		/* margin-right: 20rpx; */
		width: 400rpx;
		color: #909090;
	}

	.person_description text {
		width: 100%;
		font-size: 30rpx;
		display: -webkit-box;
		/*弹性伸缩盒子模型显示*/
		-webkit-box-orient: vertical;
		/*排列方式*/
		-webkit-line-clamp: 1;
		/*显示文本行数*/
		overflow: hidden;
		/*溢出隐藏*/
	}

	.information_card {
		margin: 0;
		padding: 10rpx;
	}

	.information_container {
		width: 95%;
	}

	.information_title {
		color: #606060;
		font-size: 30rpx;
	}

	.information_text {
		font-size: 30rpx;
	}

	.information_edit {
		height: 40rpx;
		width: 40rpx;
		margin-top: 30rpx;
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

	.unfollow {
		position: absolute;
		right: 30rpx;
		font-size: 40rpx;
		color: white;
		margin-top: 30rpx;
		width: 200rpx;
		height: 60rpx;
		text-align: center;
		background: #83cbac;
	}

	.follow {
		position: absolute;
		right: 30rpx;
		font-size: 40rpx;
		color: white;
		margin-top: 30rpx;
		width: 200rpx;
		height: 60rpx;
		text-align: center;
		background: #b5aa90;
		/* box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19); */
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
</style>
