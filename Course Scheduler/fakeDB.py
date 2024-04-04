import sqlite3
from faker import Faker
import random

# Initialize Faker
fake = Faker()

# Connect to your database
conn = sqlite3.connect('Course_SchedulerDB.db')
c = conn.cursor()

# Number of records you want to create
num_teachers = 100
num_courses = 500
num_schedules = 1000

# Generate fake Teachers
for _ in range(num_teachers):
    c.execute("INSERT INTO Teachers (name, preferredTime) VALUES (?, ?)",
              (fake.name(), fake.day_of_week() + " " + fake.time()))

# Generate fake Courses
for _ in range(num_courses):
    teacher_id = random.randint(1, num_teachers)
    c.execute("INSERT INTO Courses (name, teacherId) VALUES (?, ?)",
              (fake.word().capitalize(), teacher_id))

# Generate fake Schedules
for _ in range(num_schedules):
    course_id = random.randint(1, num_courses)
    class_time = fake.day_of_week() + " " + fake.time()
    c.execute("INSERT INTO Schedules (courseId, classTime) VALUES (?, ?)",
              (course_id, class_time))

# Commit the changes and close the connection
conn.commit()
conn.close()