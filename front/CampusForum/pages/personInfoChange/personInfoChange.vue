<template>
	<view>
		<view class="avater_container">
			<image class="avater" :src="user.avater" mode="aspectFill" @click="chooseAndUploadPic()"></image>
			<input placeholder="请输入用户名" maxlength="6" class="name" type="text" v-model="user.name" />
		</view>
		<view>
			<input placeholder="请输入个性签名" maxlength="50" class="description" type="text" v-model="user.description" />
		</view>
		<view class="person_card">
			<view class="information_card flex_box">
				<view>
					<image class="information_edit" src="../../static/college.png" mode="aspectFill"></image>
				</view>
				<view class="information_container">
					<view class="information_title">
						<text>学院</text>
					</view>
					<view class="information_text">
						<input maxlength=20 type="text" placeholder="请输入学院" v-model="user.college" />
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view>
					<image class="information_edit" src="../../static/college.png" mode="aspectFill"></image>
				</view>
				<view class="information_container">
					<view class="information_title">
						<text>系别</text>
					</view>
					<view class="information_text">
						<input maxlength=20 type="text" placeholder="请输入系" v-model="user.department" />
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view>
					<image class="information_edit" src="../../static/gender.png" mode="aspectFill"></image>
				</view>
				<view class="information_container">
					<view class="information_title">
						<text>性别</text>
					</view>
					<view class="information_text">
						<view class="uni-list-cell-db">
							<picker @change="bindGenderChange" :value="user.gender" :range="genderArray">
								<view class="uni-input">{{ genderArray[user.gender] }}</view>
							</picker>
						</view>
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view>
					<image class="information_edit" src="../../static/phone.png" mode="aspectFill"></image>
				</view>
				<view class="information_container">
					<view class="information_title">
						<text>电话</text>
					</view>
					<view class="information_text">
						<input maxlength=20 type="text" placeholder="请输入电话" v-model="user.phone" />
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view>
					<image class="information_edit" src="../../static/mail.png" mode="aspectFill"></image>
				</view>
				<view class="information_container">
					<view class="information_title">
						<text>邮箱</text>
					</view>
					<view class="information_text">
						<input maxlength=50 type="text" placeholder="请输入邮箱" v-model="user.email" />
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view>
					<image class="information_edit" src="../../static/birthday.png" mode="aspectFill"></image>
				</view>
				<view class="information_container">
					<view class="information_title">
						<text>生日</text>
					</view>
					<view class="information_text">
						<view class="uni-list-cell-db">
							<picker mode="date" :value="user.birthday" @change="bindDateChange">
								<view class="uni-input">{{ user.birthday }}</view>
							</picker>
						</view>
					</view>
				</view>
			</view>
		</view>
		
		<view class="person_card">
			<view class="information_card flex_box">
				<view>
					<image class="information_edit" src="../../static/school.png" mode="aspectFill"></image>
				</view>
				<view class="information_container">
					<view class="information_title">
						<text>小学</text>
					</view>
					<view class="information_text">
						<input maxlength=20 type="text" placeholder="请输入小学" v-model="user.primarySchool" />
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view>
					<image class="information_edit" src="../../static/school.png" mode="aspectFill"></image>
				</view>
				<view class="information_container">
					<view class="information_title">
						<text>初中</text>
					</view>
					<view class="information_text">
						<input maxlength=20 type="text" placeholder="请输入初中" v-model="user.juniorHighSchool" />
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view>
					<image class="information_edit" src="../../static/school.png" mode="aspectFill"></image>
				</view>
				<view class="information_container">
					<view class="information_title">
						<text>高中</text>
					</view>
					<view class="information_text">
						<input maxlength=50 type="text" placeholder="请输入高中" v-model="user.highSchool" />
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view>
					<image class="information_edit" src="../../static/school.png" mode="aspectFill"></image>
				</view>
				<view class="information_container">
					<view class="information_title">
						<text>大学</text>
					</view>
					<view class="information_text">
						<input maxlength=50 type="text" placeholder="请输入大学" v-model="user.university" />
					</view>
				</view>
			</view>
		</view>
		
		<view class="person_card">
			<view class="information_card flex_box">
				<view>
					<image class="information_edit" src="../../static/other.png" mode="aspectFill"></image>
				</view>
				<view class="information_container">
					<view class="information_title">
						<text>爱好</text>
					</view>
					<view class="information_text">
						<input maxlength=20 type="text" placeholder="请输入爱好" v-model="user.hobby.other" />
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view>
					<image class="information_edit" src="../../static/music.png" mode="aspectFill"></image>
				</view>
				<view class="information_container">
					<view class="information_title">
						<text>喜欢的音乐</text>
					</view>
					<view class="information_text">
						<input maxlength=20 type="text" placeholder="请输入喜欢的音乐" v-model="user.hobby.music" />
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view>
					<image class="information_edit" src="../../static/book.png" mode="aspectFill"></image>
				</view>
				<view class="information_container">
					<view class="information_title">
						<text>喜欢的书籍</text>
					</view>
					<view class="information_text">
						<input maxlength=20 type="text" placeholder="请输入喜欢的书籍" v-model="user.hobby.book" />
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view>
					<image class="information_edit" src="../../static/movie.png" mode="aspectFill"></image>
				</view>
				<view class="information_container">
					<view class="information_title">
						<text>喜欢的电影</text>
					</view>
					<view class="information_text">
						<input maxlength=20 type="text" placeholder="请输入喜欢的电影" v-model="user.hobby.movie" />
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view>
					<image class="information_edit" src="../../static/game.png" mode="aspectFill"></image>
				</view>
				<view class="information_container">
					<view class="information_title">
						<text>喜欢的游戏</text>
					</view>
					<view class="information_text">
						<input maxlength=20 type="text" placeholder="请输入喜欢的游戏" v-model="user.hobby.game" />
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view>
					<image class="information_edit" src="../../static/comic.png" mode="aspectFill"></image>
				</view>
				<view class="information_container">
					<view class="information_title">
						<text>喜欢的动漫</text>
					</view>
					<view class="information_text">
						<input maxlength=20 type="text" placeholder="请输入喜欢的动漫" v-model="user.hobby.comic" />
					</view>
				</view>
			</view>
			<view class="information_card flex_box">
				<view>
					<image class="information_edit" src="../../static/sport.png" mode="aspectFill"></image>
				</view>
				<view class="information_container">
					<view class="information_title">
						<text>喜欢的运动</text>
					</view>
					<view class="information_text">
						<input maxlength=20 type="text" placeholder="请输入喜欢的运动" v-model="user.hobby.sport" />
					</view>
				</view>
			</view>
		</view>

		<button class="register_button" @click="update()">更 新</button>

		<uni-popup ref="popup_error" type="message">
			<uni-popup-message type="error" message="用户名不能为空" :duration="2000"></uni-popup-message>
		</uni-popup>
		<uni-popup ref="popup_error_password" type="message">
			<uni-popup-message type="error" message="密码长度需在6-16之间" :duration="2000"></uni-popup-message>
		</uni-popup>
		<uni-popup ref="popup_error_email" type="message">
			<uni-popup-message type="error" message="邮箱格式不正确" :duration="2000"></uni-popup-message>
		</uni-popup>
		<uni-popup ref="popup_error_phone" type="message">
			<uni-popup-message type="error" message="电话格式不正确" :duration="2000"></uni-popup-message>
		</uni-popup>
		<uni-popup ref="popup_success" type="message">
			<uni-popup-message type="success" message="修改成功" :duration="2000"></uni-popup-message>
		</uni-popup>
	</view>
</template>

<script>
	import * as userApi from "../../api/user.js"
	import * as config from "../../utils/config.js"

	export default {
		data() {
			return {
				genderArray: [
					'保密',
					'男',
					'女'
				],
				user: {
					studentId: 22920191234,
					name: '墨羽翎玖',
					college: '信息学院',
					department: '',
					gender: 1,
					avater: '../../static/avater.jpg',
					description: '来时山有雪，归时雪满山。',
					birthday: '2021-12-15',
					phone: '12312341234',
					email: '123456@qq.com',
					following: 100,
					follower: 100,
					primarySchool: '',
					juniorHighSchool: '',
					highSchool: '',
					university: '',
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
				}
			}
		},
		onLoad(prop) {
			if (config.checkToken()) {
				userApi.select().then(data => {
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
						this.user.id = data.data.id
						this.user.studentId = data.data.studentId
						this.user.name = data.data.name
						this.user.college = data.data.college
						this.user.department = data.data.department
						this.user.gender = data.data.gender
						this.user.avater = "/api"+String(data.data.avater).replace(/\\/g, "/")
						this.user.description = data.data.description
						this.user.phone = data.data.phone
						this.user.email = data.data.email

						this.user.following = data.data.following
						this.user.follower = data.data.follower

						this.user.gmtCreate = data.data.gmt_create
						this.user.gmtModified = data.data.gmt_modified

						this.user.admin = data.data.admin

						this.user.primarySchool = data.data.primarySchool
						this.user.juniorHighSchool = data.data.juniorHighSchool
						this.user.highSchool = data.data.highSchool
						this.user.university = data.data.university
						
						this.user.hobby.other = data.data.hobby.other
						this.user.hobby.music = data.data.hobby.music
						this.user.hobby.book = data.data.hobby.book
						this.user.hobby.movie = data.data.hobby.movie
						this.user.hobby.game = data.data.hobby.game
						this.user.hobby.comic = data.data.hobby.comic
						this.user.hobby.sport = data.data.hobby.sport
					}
				})
			} else {
				uni.redirectTo({
					url: '../login/login'
				})
			}
		},
		methods: {
			getGender() {
				return config.getGender(this.user.gender)
			},
			update() {
				if (this.user.name.length == 0) {
					this.$refs.popup_error.open('top')
				} else if (this.user.phone.length != 0 && !config.checkPhone(this.user.phone)) {
					this.$refs.popup_error_phone.open('top')
				} else if (this.user.email.length != 0 && !config.checkEmail(this.user.email)) {
					this.$refs.popup_error_email.open('top')
				} else {
					userApi.updateById(this.user.id, this.user).then(data => {
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
						} else if (data.code != 200) {
							uni.showToast({
								title: data.msg,
								icon: "error",
								mask: true,
								duration: 2000
							})
						} else {
							let token = data.data.token
							config.saveTokenFroce(token)
							this.$refs.popup_success.open('top')
							setTimeout(() => {
								uni.reLaunch({
									url: '../person/person'
								})
							}, config.waitTime)
						}
					})
				}
			},
			bindDateChange(e) { // 改变日期
				this.user.birthday = e.target.value
			},
			getToday() { // 获取今天
				const date = new Date();
				let year = date.getFullYear();
				let month = date.getMonth() + 1;
				let day = date.getDate();
				month = month > 9 ? month : '0' + month;
				day = day > 9 ? day : '0' + day;
				return `${year}-${month}-${day}`;
			},
			bindGenderChange(e) { // 改变性别
				this.user.gender = e.target.value
			},
			chooseAndUploadPic() {
				uni.chooseImage({
					count: 1,
					sizeType: ['copressed'],
					success(res) {
						//因为有一张图片， 输出下标[0]， 直接输出地址
						var imgFiles = res.tempFilePaths[0]

						console.log(imgFiles)
						console.log(config.getToken())
						// 上传图片
						// 做成一个上传对象
						var uper = uni.uploadFile({
							// 需要上传的地址
							url: '/api/picture/head/insert',
							header: {
								'token': config.getToken(),
							},
							// filePath  需要上传的文件
							filePath: imgFiles,
							name: 'file',
							success(res1) {
								// 显示上传信息
								console.log(res1)
								uni.navigateTo({
									url: '/pages/personInfoChange/personInfoChange',
								});
							}
						});
					}
				})

			},
		}
	}
</script>

<style>
	.flex_box {
		display: flex;
	}

	.avater_container {
		margin-top: 20rpx;
		text-align: center;
	}

	.avater {
		width: 300rpx;
		height: 300rpx;
		border-radius: 50%;
	}

	.name {
		width: 40%;
		margin-left: 30%;
		font-size: 40rpx;
		font-weight: bold;
		/* border-bottom: 2rpx solid #A0A0A0; */
		border-bottom: none;
	}

	.description {
		width: 40%;
		margin-top: 10rpx;
		margin-left: 30%;
		font-size: 25rpx;
		color: #808080;
		/* border-bottom: 2rpx solid #A0A0A0; */
		border-bottom: none;
		text-align: center;
	}

	.name:hover {
		border-bottom: 2rpx solid #A0C2E7;
	}

	.register_button {
		width: 90%;
		margin: 30rpx 5%;
		background-image: linear-gradient(to top, #48c6ef 0%, #6f86d6 100%);
		font-weight: bold;
		border-radius: 40rpx;
		box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
	}

	.login_button {
		width: 90%;
		margin: 30rpx 5%;
		background-image: linear-gradient(120deg, #f6d365 0%, #fda085 100%);
		font-weight: bold;
		border-radius: 40rpx;
		box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
	}

	.person_card {
		width: 90%;
		margin-left: 5%;
		margin-right: 5%;
		margin-top: 40rpx;
		/* height: 160rpx; */
		background-color: #FFFFFF;
		padding-left: 10rpx;
		padding-top: 10rpx;
		padding-bottom: 10rpx;
		border-radius: 10rpx;
		box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
	}

	.information_card {
		margin: 0;
		padding: 10rpx;
	}

	.information_container {
		display: flex;
		width: 95%;
		margin-left: 10rpx;
	}

	.information_title {
		color: #606060;
		font-size: 35rpx;
	}

	.information_text {
		margin-left: 20rpx;
		font-size: 35rpx;
	}

	.information_edit {
		height: 30rpx;
		width: 30rpx;
		margin-top: 10rpx;
	}
</style>
